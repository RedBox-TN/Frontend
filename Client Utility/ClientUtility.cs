using Blazored.SessionStorage;
using Frontend.Channel_Utility;
using Frontend.Token_Utility;

namespace Frontend.Client_Utility;

public class ClientUtility
{
    private readonly ChannelUtility _channelUtility;
    private readonly ISyncSessionStorageService _syncSessionStorage;
    private readonly TokenUtility _tokenUtility;

    public ClientUtility(ISyncSessionStorageService syncSessionStorage, ChannelUtility channelUtility,
        TokenUtility tokenUtility)
    {
        _syncSessionStorage = syncSessionStorage;
        _channelUtility = channelUtility;
        _tokenUtility = tokenUtility;
    }

    public bool IsLoggedIn()
    {
        return _channelUtility.GetHttpClient().DefaultRequestHeaders.Authorization != null;
    }
}