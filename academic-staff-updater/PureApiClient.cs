using System.Net;
using System.Text;
using Newtonsoft.Json;
using academic_staff_updater.PureContracts;
using System;
using System.Linq;

namespace academic_staff_updater
{
    class PureApiClient
    {
        private string apiKey;
        private string baseUrl;

        public PureApiClient(string apiKey, string baseUrl)
        {
            this.apiKey = apiKey;
            this.baseUrl = baseUrl;
        }

        //public List<AcademicStaff> GetPersons()
        //{
        //    string query = ""


        //}

        public string GetResearchRenderingForStaff(AcademicStaff staff)
        {
            string staffResearchRendering = "";
            var fields = "publicationStatuses.publicationDate.*";
            var rederStyle = "apa";

            var endpoint = $"/persons/{staff.PureId}/research-outputs?idClassification=PURE&fields={fields}&rendering={rederStyle}&navigationLink=false";
            Console.WriteLine(endpoint);
            var response = GetResponse<ResearchOutputResponse>(endpoint);
            if(response != null && response.Items.Count >= 0)
            {
                
                ResearchItem[] outputs = response.Items.OrderByDescending(i => i.Statuses.First().Date.GetDate()).ToArray();
                var sb = new StringBuilder();
                foreach (var o in outputs)
                {
                    sb.Append(o.Renderings.First().Html);
                }
                staffResearchRendering = sb.ToString();
            }

            return staffResearchRendering;

        }

        private T GetResponse<T>(string endpoint)
        {
            T response = default(T);

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("api-key", apiKey);
                    Console.WriteLine(apiKey);
                    //client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Accept", "application/json");
                     
                    string result = client.DownloadString(baseUrl + endpoint);
                    response = JsonConvert.DeserializeObject<T>(result);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    // nothing happes - which means response == null
                }
            }

            return response;

        }
    }
}
