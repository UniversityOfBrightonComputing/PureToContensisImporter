﻿using System.Configuration;

namespace academic_staff_updater
{
    internal static class PureClientFactory
    {
        //Contensis target client Settings
        private static string apiBaseUrl = ConfigurationManager.AppSettings["pureApiBaseUrl"];
        private static string apiKey = ConfigurationManager.AppSettings["pureApiKey"];

        public static PureApiClient GetClient()
        {
            return new PureApiClient(apiKey, apiBaseUrl);
        }
    }
}
