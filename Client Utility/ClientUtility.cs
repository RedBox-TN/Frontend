using System.Net.Http.Headers;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace Frontend.Client_Utility;

public class ClientUtility
{
    private HttpClient _httpClient;
    private GrpcChannel _channel;

    public ClientUtility()
    {
        _httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        _channel = GrpcChannel.ForAddress("http://localhost:5200", new GrpcChannelOptions { HttpClient = _httpClient });
    }

    public GrpcChannel GetChannel()
    {
        return _channel;
    }

    public void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        _channel = GrpcChannel.ForAddress("http://localhost:5200", new GrpcChannelOptions{ HttpClient = _httpClient });
    }
}