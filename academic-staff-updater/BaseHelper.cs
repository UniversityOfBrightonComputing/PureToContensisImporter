using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace academic_staff_updater
{
    class BaseHelper
    {

        //Contensis target client Settings
        public static string clientRootUrl = ConfigurationManager.AppSettings["contensisCmsRootUrl"];
        public static string clientId = ConfigurationManager.AppSettings["contensisApiClientId"];
        public static string clientSharedSecret = ConfigurationManager.AppSettings["contensisApiSharedSecret"];

        //Contensis target CMS settings
        public static string targetProject = ConfigurationManager.AppSettings["targetProject"];
        public static string targetContentType = ConfigurationManager.AppSettings["targetContentType"];
    }
}
