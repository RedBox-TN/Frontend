using System.Net.Http.Headers;
using Frontend.Settings;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Options;

namespace Frontend.Client_Utility;

public class ClientUtility
{
	private readonly HttpClient _httpClient;
	private readonly RedBoxBackEndSettings _options;
	private GrpcChannel _channel;

	public ClientUtility(IOptions<RedBoxBackEndSettings> options)
	{
		_options = options.Value;
		_httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
		_channel = GrpcChannel.ForAddress(_options.BackEndUrl, new GrpcChannelOptions
		{
			HttpClient = _httpClient
		});
	}

	public GrpcChannel GetChannel()
	{
		return _channel;
	}

	public void SetAuthToken(string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
		_channel = GrpcChannel.ForAddress(_options.BackEndUrl, new GrpcChannelOptions
		{
			HttpClient = _httpClient
		});
	}

	public void UnsetAuthToken()
	{
		_httpClient.DefaultRequestHeaders.Authorization = null;
		_channel = GrpcChannel.ForAddress(_options.BackEndUrl, new GrpcChannelOptions
		{
			HttpClient = _httpClient
		});
	}

	public bool IsLoggedIn()
	{
		return _httpClient.DefaultRequestHeaders.Authorization is not null;
	}
}