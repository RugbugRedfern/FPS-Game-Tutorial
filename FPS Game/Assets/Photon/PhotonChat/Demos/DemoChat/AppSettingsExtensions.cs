using Photon.Chat;
using Photon.Realtime;

public static class AppSettingsExtensions 
{
    public static ChatAppSettings GetChatSettings(this AppSettings appSettings)
    {
        return new ChatAppSettings
        {
            AppId = appSettings.AppIdChat,
            AppVersion = appSettings.AppVersion,
            FixedRegion = appSettings.IsBestRegion ? null : appSettings.FixedRegion,
            NetworkLogging = appSettings.NetworkLogging,
            Protocol = appSettings.Protocol,
            Server = appSettings.IsDefaultNameServer ? null : appSettings.Server
        };
    }
}
