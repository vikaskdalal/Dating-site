using DotNetCoreAngular.Common;

namespace DotNetCoreAngular.Helpers
{
    public class SignalRHelper
    {
        public static string GetGroupCacheKey(string groupName)
        {
            return $"{InMemoryCacheKeys.SIGNALR_GROUP}_{groupName}";
        }
    }
}
