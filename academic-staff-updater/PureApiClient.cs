using Newtonsoft.Json;
using System;
using RestSharp;
using System.Collections.Generic;

namespace academic_staff_updater
{
    class PureApiClient
    {
        private string ApiKey;
        private string BaseUrl;

        public PureApiClient(string apiKey, string baseUrl)
        {
            ApiKey = apiKey;
            BaseUrl = baseUrl;
        }

        public List<AcademicStaff> GetAcademicStaff()
        {
            var staffList = new List<AcademicStaff>();
            List<PersonsResponse> responses = GetResponses<PersonsResponse>(() => GetBasePersonsRequest(100));
            foreach(var response in responses)
            {
                foreach(var person in response.items)
                {
                    var staff = new AcademicStaff
                    {
                        Id = person.Id,
                        Title = person.Title,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Email = person.Email
                    };
                    staffList.Add(staff);
                }   
            }
            return staffList;
        }

        public List<T> GetResponses<T>(Func<RestRequest> baseRequester) where T : IPureApiPagedResponse
        {
            bool morePages = true;
            int pageSize = 10;
            int currentOffset = 0;

            var responses = new List<T>();
            var client = new RestClient(BaseUrl);

            while (morePages)
            {
                var request = baseRequester();
                request.AddQueryParameter("offset", currentOffset.ToString());

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = JsonConvert.DeserializeObject<T>(response.Content);
                    if (content != null)
                    {
                        responses.Add(content);
                        morePages = content.MorePages();
                        currentOffset += pageSize;
                    }
                    else
                    {
                        morePages = false;
                    }
                }
                else
                {
                    morePages = false;
                }
            }
            return responses;
        }

        public RestRequest GetBasePersonsRequest(int pageSize)
        {
            var request = new RestRequest("/persons", Method.POST);
            AddRestHeaders(request);

            var jsonParams = new
            {
                employmentStatus = "ACTIVE",
                navigationLink = true,
                size = pageSize,
                fields = new string[] {
                    "pureId",
                    "name.firstName",
                    "name.lastName",
                    "titles.value",
                    "staffOrganisationAssociations.emails.value",
                }
            };
            request.AddJsonBody(jsonParams);

            return request;
        }

        public RestRequest GetBaseReseachOutputsRequest(string email, int size)
        {
            var request = new RestRequest($"/persons/{email}/research-outputs");
            AddRestHeaders(request);

            request.AddQueryParameter("idClassification", "email");
            request.AddQueryParameter("rendering", "apa");
            request.AddQueryParameter("fields", "rendering");
            request.AddQueryParameter("order", "-publicationYear");
            request.AddQueryParameter("size", size.ToString());

            return request;
        }

        private void AddRestHeaders(IRestRequest request)
        {
            request.AddHeader("api-key", ApiKey);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
        }

    }
}




