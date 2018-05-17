using System.Configuration;
using Zengenti.Contensis.Management;

namespace academic_staff_updater.Contensis
{
    internal static class ContensisClientFactory
    {
        //Contensis target client Settings
        private static string clientRootUrl = ConfigurationManager.AppSettings["contensisCmsRootUrl"];
        private static string clientId = ConfigurationManager.AppSettings["contensisApiClientId"];
        private static string clientSharedSecret = ConfigurationManager.AppSettings["contensisApiSharedSecret"];

        public static ManagementClient GetClient()
        {
            return ManagementClient.Create(
                clientRootUrl,
                clientId,
                clientSharedSecret
            );
        }
    }
}
