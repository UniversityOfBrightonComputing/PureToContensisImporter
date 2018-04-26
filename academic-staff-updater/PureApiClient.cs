using System.Net;
using System.Text;
using Newtonsoft.Json;
using academic_staff_updater.PureContracts;
using System;
using System.Linq;
using RestSharp;
using System.Collections.Generic;
using System.IO;

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

        public string GetPersons()
        {
            var jsonParams = new
            {
                employmentStatus = "ACTIVE",
                navigationLink = false,
                size = 5000,
                fields = new string[] {
                    "uuid",
                    "name.firstName",
                    "name.lastName",
                    "titles.value",
                    "staffOrganisationAssociations.emails.value",
                }
            };
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseUrl + "/persons"));
            http.Headers["api-key"] = apiKey;
            http.Accept = "application/json";
            http.ContentType = "application/json; charset=utf-8";
            http.Method = "POST";

             string parsedContent = JsonConvert.SerializeObject(jsonParams);
            //string parsedContent = "{\"fields\": [\"PureId\", \"titles.value\",\"name.firstName\",\"name.lastName\",\"staffOrganisationAssociations.emails.value\"],\"employmentStatus\": \"ACTIVE\",\"navigationLink\": false,\"size\": 5000}";

            Byte[] bytes = Encoding.UTF8.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            var content = sr.ReadToEnd();
            return content;
        }

        public List<AcademicStaff> GetAcademicStaff()
        {
            var academicStaff = new List<AcademicStaff>();
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/persons", Method.POST);
            var jsonParams = new
            {
                employmentStatus = "ACTIVE",
                navigationLink = false,
                size = 1,
                fields = new string[] {
                    "uuid",
                    "name.firstName",
                    "name.lastName",
                    "titles.value",
                    "staffOrganisationAssociations.emails.value",                    
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
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var person in response.Data.Items)
                    {
                        Console.WriteLine($"{person.Name.FirstName} {person.Name.LastName} - {person.PureId}");
                        //var newStaff = ConvertPersonToAcademicStaff(person);
                        //academicStaff.Add(newStaff);
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

        public AcademicStaff ConvertPersonToAcademicStaff(PersonItem person) => new AcademicStaff
        {
            PureId = person.PureId.ToString(),
            Title = person.Title,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Email = person.Email
        };

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
