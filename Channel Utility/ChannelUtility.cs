using System.Net.Http.Headers;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace Frontend.Channel_Utility;

public class ChannelUtility
{
    private const string BackEndUrl = "https://api.redbox.benaco2000.ovh";
    private readonly HttpClient _httpClient;
    private GrpcChannel _channel;

    public ChannelUtility()
    {
        _httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        _channel = GrpcChannel.ForAddress(BackEndUrl, new GrpcChannelOptions
        {
            HttpClient = _httpClient
        });
    }

    public GrpcChannel GetChannel()
    {
        return _channel;
    }

    public HttpClient GetHttpClient()
    {
        return _httpClient;
    }

    public void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        _channel = GrpcChannel.ForAddress(BackEndUrl, new GrpcChannelOptions
        {
            HttpClient = _httpClient
        });
    }

    public void UnsetAuthToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
        _channel = GrpcChannel.ForAddress(BackEndUrl, new GrpcChannelOptions
        {
            HttpClient = _httpClient
        });
    }
}