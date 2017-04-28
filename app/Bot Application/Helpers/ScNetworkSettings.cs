
using Sitecore.MobileSDK.API;
using Sitecore.MobileSDK.API.Session;
using Sitecore.MobileSDK.PasswordProvider;
using Sitecore.MobileSDK.PasswordProvider.Interface;

namespace SCHelpers
{
    public static class ScNetworkSettings
    {
        private static string defaultDatabase = "web";
        public static string instanceUrl = "http://cms82u1.test24dk1.dk.sitecore.net/";

        public static IScCredentials Credentials()
        {
            return new ScUnsecuredCredentialsProvider("admin", "b", "sitecore");
        }

        public static ISitecoreSSCSession Session(IScCredentials credentials)
        {
            return SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(instanceUrl)
                                                              .Credentials(credentials)
                                                              .DefaultDatabase(defaultDatabase)
                                                              .BuildSession();
        }

    }
}