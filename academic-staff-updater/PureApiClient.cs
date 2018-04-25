using System.Net;
using System.Text;
using Newtonsoft.Json;
using academic_staff_updater.PureContracts;
using System;
using System.Linq;
using RestSharp;
using System.Collections.Generic;

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

        public List<AcademicStaff> GetAcademicStaff()
        {
            var academicStaff = new List<AcademicStaff>();
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/persons", Method.POST);
            var jsonParams = new
            {
                employmentStatus = "ACTIVE",
                size = 5000,
                fields = new string[] {
                    "name.firstName",
                    "name.lastName",
                    "pureId",
                    "staffOrganisationAssociations.emails.value",
                    "titles.value"
                }
            };
            //Console.WriteLine(jsonParams);
            request.AddJsonBody(jsonParams);
            request.AddHeader("api-key", apiKey);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");


            try
            {
                var response = client.Execute<PersonsResponse>(request);
                Console.WriteLine($"Found {response.Data.Count} people");
                if (response.Data.Count > 0)
                {
                    foreach (var person in response.Data.Items)
                    {
                        Console.WriteLine($"{person.Name.FirstName} {person.Name.LastName}");
                        academicStaff.Add(new AcademicStaff(
                            person.PureId.ToString(),
                            person.Titles.First().Value,
                            person.Name.FirstName,
                            person.Name.LastName
                        ));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }


            return academicStaff;

        }

        public string GetResearchRenderingForStaff(AcademicStaff staff)
        {
            string staffResearchRendering = "";
            var fields = "publicationStatuses.publicationDate.*";
            var renderStyle = "apa";

            var endpoint = $"/persons/{staff.PureId}/research-outputs?idClassification=PURE&fields={fields}&rendering={renderStyle}&navigationLink=false";
            Console.WriteLine(endpoint);
            var response = GetResponse<ResearchOutputResponse>(endpoint);
            if (response != null && response.Items.Count >= 0)
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
                    client.Headers.Add("Accept", "application/json");
                    client.Headers.Add("Content-Type", "application/json");


                    string result = client.DownloadString(baseUrl + endpoint);
                    response = JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    // nothing happes - which means response == null
                }
            }

            return response;

        }
    }
}
