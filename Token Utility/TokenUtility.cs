using Blazored.SessionStorage;
using Frontend.Channel_Utility;
using Google.Protobuf.WellKnownTypes;
using RedBoxAuthentication;

namespace Frontend.Token_Utility;

public class TokenUtility
{
	private readonly AuthenticationGrpcService.AuthenticationGrpcServiceClient _apiAuth;
	private readonly ChannelUtility _channelUtility;
	private readonly ISyncSessionStorageService _syncSessionStorage;
	private readonly CancellationTokenSource _tokenTaskCancel = new();
	private Task? _tokenTask;

	public TokenUtility(AuthenticationGrpcService.AuthenticationGrpcServiceClient apiAuth,
		ISessionStorageService sessionStorage, ChannelUtility channelUtility,
		ISyncSessionStorageService syncSessionStorage)
	{
		_apiAuth = apiAuth;
		_channelUtility = channelUtility;
		_syncSessionStorage = syncSessionStorage;
		if (!syncSessionStorage.ContainKey("Token")) return;
		var token = syncSessionStorage.GetItemAsString("Token");
		var expiry = syncSessionStorage.GetItem<long>("Expiry");
		_channelUtility.SetAuthToken(token);
		_tokenTask ??= Task.Run(() => RefreshTokenTask(new TokenRefreshResponse
		{
			Token = token,
			ExpiresAt = expiry
		}, _tokenTaskCancel.Token), _tokenTaskCancel.Token);
	}

	private async Task RefreshTokenTask(TokenRefreshResponse token, CancellationToken ct = default)
	{
		while (!ct.IsCancellationRequested)
		{
			_syncSessionStorage.SetItemAsString("Token", token.Token);
			_syncSessionStorage.SetItem("Expiry", token.ExpiresAt);
			try
			{
				await Task.Delay((int)(token.ExpiresAt - DateTimeOffset.Now.ToUnixTimeMilliseconds() - 60000), ct);
			}
			catch (TaskCanceledException)
			{
			}

			token = _apiAuth.RefreshToken(new Empty(), cancellationToken: ct);
			_channelUtility.SetAuthToken(token.Token);
		}

		_channelUtility.UnsetAuthToken();
		_syncSessionStorage.Clear();
	}

	public Task RefreshToken(TokenRefreshResponse token)
	{
		_tokenTask ??= Task.Run(() => RefreshTokenTask(token, _tokenTaskCancel.Token), _tokenTaskCancel.Token);

		return Task.CompletedTask;
	}

	public void ClearToken()
	{
		if (_tokenTask == null) return;
		_tokenTaskCancel.Cancel();
		while (_tokenTask.IsCanceled)
		{
		}
	}
}