using System.Configuration;
using Zengenti.Contensis.Management;

namespace academic_staff_updater.Contensis
{
    internal static class CmsClientFactory
    {
        public static CmsClient GetClient()
        {
            ManagementClient contensisClient = ContensisClientFactory.GetClient();
            string targetProject = ConfigurationManager.AppSettings["cmsMainProject"];
            var project = contensisClient.Projects.Get(targetProject);

            return new CmsClient(contensisClient, project);
        }
    }
}
