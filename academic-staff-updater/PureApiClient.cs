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

        public PersonsResponse GetPersons()
        {
            PersonsResponse personsResponse = null;
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
            request.AddJsonBody(jsonParams);
            AddRestHeaders(request);

            try
            {
                var response = client.Execute<PersonsResponse>(request);
                personsResponse = response.Data;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
            return personsResponse;
        }

        public List<AcademicStaff> GetAcademicStaff()
        {
            var academicStaff = new List<AcademicStaff>();
            PersonsResponse response = GetPersons();
            if(response != null)
            {
                foreach(var person in response.Items)
                {
                    var newStaff = ConvertPersonToAcademicStaff(person);
                    academicStaff.Add(newStaff);
                }
            }
            return academicStaff;
        }

        public string GetResearchRenderingForStaff(AcademicStaff staff)
        {
            string staffResearchRendering = "";
            var fields = "publicationStatuses.publicationDate.*";
            var renderStyle = "apa";

            var endpoint = $"/persons/{staff.Id}/research-outputs?idClassification=PURE&fields={fields}&rendering={renderStyle}&navigationLink=false";
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
            Id = person.Id,
            Title = person.Title,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Email = person.Email
        };

        private void AddRestHeaders(IRestRequest request)
        {
            request.AddHeader("api-key", apiKey);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
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
