using Blazored.SessionStorage;
using Frontend.Channel_Utility;
using Frontend.Token_Utility;
using Grpc.Health.V1;

namespace Frontend.Client_Utility;

public class ClientUtility
{
	private readonly ChannelUtility _channelUtility;
	private readonly Health.HealthClient _client;


	public ClientUtility(ChannelUtility channelUtility)
	{
		_channelUtility = channelUtility;
		_client = new Health.HealthClient(_channelUtility.GetChannel());
	}

	public async Task<bool> IsLoggedIn()
	{
		return await Task.Run(() => _channelUtility.GetHttpClient().DefaultRequestHeaders.Authorization != null);
	}

	public async Task<bool> IsServerOn()
	{
		try
		{
			var response = await _client.CheckAsync(new HealthCheckRequest());
			var status = response.Status;
			return status == HealthCheckResponse.Types.ServingStatus.Serving;
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return false;
		}
	}
}