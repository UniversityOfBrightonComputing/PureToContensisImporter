using Newtonsoft.Json;
using System;
using RestSharp;
using System.Collections.Generic;

namespace PureToContensisImporter.Pure
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

        public List<PersonsResponse.Person> GetPersons(int limit, int pageSize = 100)
        {
            List<PersonsResponse> responses = GetResponses<PersonsResponse>(
                () => GetBasePersonsRequest(pageSize),
                limit
            );
            var persons = new List<PersonsResponse.Person>();
            foreach (var response in responses)
            {
                foreach (var item in response.items)
                {
                    persons.Add(item);
                }
            }
            return persons;
        }

        public List<ResearchOutputsResponse.Output> GetResearchOutputsForEmail(string encodedEmail, int limit, int pageSize = 25)
        {
            List<ResearchOutputsResponse> responses = GetResponses<ResearchOutputsResponse>(
                () => GetBaseReseachOutputsRequest(encodedEmail, pageSize),
                limit
            );
            var outputs = new List<ResearchOutputsResponse.Output>();
            foreach (var response in responses)
            {
                foreach (var item in response.items)
                {
                    outputs.Add(item);
                }
            }
            return outputs;
        }


        public List<T> GetResponses<T>(Func<RestRequest> baseRequester, int limit) where T : PureApiResponse
        {
            bool morePages = true;
            int pageSize = 25;
            int currentOffset = 0;

            var responses = new List<T>();
            var client = new RestClient(BaseUrl);

            while (morePages && currentOffset < limit )
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
            return GetBaseReseachOutputsRequestById(email, "email", size);
        }

        public RestRequest GetBaseReseachOutputsRequest(int pureId, int size)
        {
            return GetBaseReseachOutputsRequestById(pureId.ToString(), "pure", size);
        }

        public RestRequest GetBaseReseachOutputsRequestById(string id, string idClassification, int size)
        {
            var request = new RestRequest($"/persons/{id}/research-outputs");
            AddRestHeaders(request);

            request.AddQueryParameter("idClassification", idClassification);
            request.AddQueryParameter("rendering", "harvard");
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




