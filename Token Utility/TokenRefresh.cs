using System.Timers;
using Frontend.Client_Utility;
using Google.Protobuf.WellKnownTypes;
using RedBoxAuthentication;
using Timer = System.Timers.Timer;

namespace Frontend.Token_Utility;

public class TokenRefresh
{
    private readonly AuthenticationGrpcService.AuthenticationGrpcServiceClient _apiAuth;
    private readonly ClientUtility _clientUtility;
    private Timer? _timer;

    public TokenRefresh(AuthenticationGrpcService.AuthenticationGrpcServiceClient apiAuth, ClientUtility clientUtility)
    {
        _apiAuth = apiAuth;
        _clientUtility = clientUtility;
    }
    
    public async Task RefreshTimer(long expiry)
    {
        _timer = new Timer();
        _timer.Interval = expiry;
        _timer.Elapsed += RefreshToken;
        _timer.Start();
    }

    private async void RefreshToken(object? sender, ElapsedEventArgs e)
    {
        var token = await _apiAuth.RefreshTokenAsync(new Empty());
        _clientUtility.SetAuthToken(token.Token);
        Console.WriteLine(token.Token);
        Console.WriteLine(token.ExpiresAt);
        _timer?.Dispose();   
        await RefreshTimer(3000);
    }
}