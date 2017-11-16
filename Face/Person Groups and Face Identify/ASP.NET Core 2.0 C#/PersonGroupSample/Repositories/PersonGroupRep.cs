using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonGroupSample.Interfaces;
using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PersonGroupSample.Repositories
{
    public class PersonGroupRep : IPersonGroupRep
    {
        private readonly AppSettings _appSettings;

        public PersonGroupRep(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<IEnumerable<PersonGroup>> GetPersonGroups()
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups";

            //make request
            var responseMessage = await httpClient.GetAsync(apiUri);

            //return object if sucess or null if not
            if (responseMessage.IsSuccessStatusCode)
            {
                //cast to array of items
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                var responseArray = JArray.Parse(responseString);
                var items = new List<PersonGroup>();
                foreach (var response in responseArray)
                {
                    var item = JsonConvert.DeserializeObject<PersonGroup>(response.ToString());
                    items.Add(item);
                }

                return items;
            }
            else
            {
                return null;
            }
        }

        public async Task<PersonGroup> GetPersonGroup(string personGroupId)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroupId}";

            //make request
            var responseMessage = await httpClient.GetAsync(apiUri);

            //return object if sucess or null if not
            if (responseMessage.IsSuccessStatusCode)
            {
                //cast to item
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<PersonGroup>(responseString);
                return item;
            }
            else
            {
                return null;
            }
        }


        public async Task<PersonGroup> CreatePersonGroup(PersonGroup personGroup)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroup.personGroupId}";

            //setup content
            var data = new Dictionary<string, string>();
            data.Add("name", personGroup.name);
            data.Add("userData", personGroup.userData);
            var dataJson = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            //make request
            var responseMessage = await httpClient.PutAsync(apiUri, content);

            //return object if sucess or null if not
            return (responseMessage.IsSuccessStatusCode) ? personGroup : null;
        }

        public async Task<PersonGroup> UpdatePersonGroup(PersonGroup personGroup)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroup.personGroupId}";

            //setup content
            var data = new Dictionary<string, string>();
            data.Add("name", personGroup.name);
            data.Add("userData", personGroup.userData);
            var dataJson = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            //make request
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), apiUri) { Content = content };
            var responseMessage = await httpClient.SendAsync(request);

            //return object if sucess or null if not
            return (responseMessage.IsSuccessStatusCode) ? personGroup : null;
        }

        public async Task<PersonGroupTrainingStatus> TrainPersonGroup(string personGroupId)
        {
            //start a trainig job
            var trainingJobResponse = await CreatePersonGroupTrainingJob(personGroupId);

            //return null if job was not started correctly
            if (!trainingJobResponse.IsSuccessStatusCode) return null;

            //get training job status and return it
            var personGroupTrainingStatus = await GetPersonGroupTrainingJobStatus(personGroupId);

            //return object
            return personGroupTrainingStatus;
        }

        private async Task<HttpResponseMessage> CreatePersonGroupTrainingJob(string personGroupId)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroupId}/train";

            //make request
            var responseMessage = await httpClient.PostAsync(apiUri, null);

            //return null if it was not a sucess otherwise return reason phrase
            return (responseMessage.IsSuccessStatusCode) ? responseMessage : null;
        }

        private async Task<PersonGroupTrainingStatus> GetPersonGroupTrainingJobStatus(string personGroupId)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroupId}/training";

            //make request
            var responseMessage = await httpClient.GetAsync(apiUri);

            //return null if it was not a sucess
            if (!responseMessage.IsSuccessStatusCode) return null;

            //cast to item
            var responseString = await responseMessage.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<PersonGroupTrainingStatus>(responseString);
            return item;
        }

        public async Task<string> DeletePersonGroup(string personGroupId)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroupId}";

            //make request
            var responseMessage = await httpClient.DeleteAsync(apiUri);

            //return reason phrase if sucess or null if not
            if (responseMessage.IsSuccessStatusCode)
            {
                return responseMessage.ReasonPhrase;
            }
            else
            {
                return null;
            }
        }
    }
}
