using Frontend.Client_Utility;
using Google.Protobuf.WellKnownTypes;
using RedBoxAuthentication;

namespace Frontend.Token_Utility;

public class TokenRefresh
{
	private readonly AuthenticationGrpcService.AuthenticationGrpcServiceClient _apiAuth;
	private readonly ClientUtility _clientUtility;
	private Task? _tokenTask;

	public TokenRefresh(AuthenticationGrpcService.AuthenticationGrpcServiceClient apiAuth, ClientUtility clientUtility)
	{
		_apiAuth = apiAuth;
		_clientUtility = clientUtility;
	}

	private async Task RefreshTokenTask(TokenRefreshResponse token)
	{
		while (true)
		{
			Console.WriteLine(token.Token);
			Console.WriteLine(token.ExpiresAt);
			Console.WriteLine(token.ExpiresAt - DateTimeOffset.Now.ToUnixTimeMilliseconds());
			await Task.Delay((int)(token.ExpiresAt - DateTimeOffset.Now.ToUnixTimeMilliseconds() - 60000));
			token = _apiAuth.RefreshToken(new Empty());
			_clientUtility.SetAuthToken(token.Token);
		}
	}

	public Task RefreshToken(TokenRefreshResponse token)
	{
		if (_tokenTask == null)
		{
			Console.WriteLine("Task null");
			_tokenTask = Task.Run(() => RefreshTokenTask(token));
		}
		else
		{
			Console.WriteLine("Task already running");
		}

		return Task.CompletedTask;
	}
}