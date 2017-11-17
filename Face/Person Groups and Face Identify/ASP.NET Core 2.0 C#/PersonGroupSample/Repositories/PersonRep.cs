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
    public class PersonRep : IPersonRep
    {
        private readonly AppSettings _appSettings;

        public PersonRep(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<string> CreatePerson(string personGroupId, Person person)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroupId}/persons";

            //setup content
            var data = new Dictionary<string, string>();
            data.Add("name", person.name);
            data.Add("userData", person.userData);
            var dataJson = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            //make request
            var responseMessage = await httpClient.PostAsync(apiUri, content);

            //return null if it was not a sucess
            if (!responseMessage.IsSuccessStatusCode) return null;

            //return object
            var personId = await responseMessage.Content.ReadAsStringAsync();
            return personId;
        }

        public async Task<string> AddPersonFace(byte[] faceImage, string personGroupId, string personId, string userData, string targetFace)
        {
            return null;
        }

        public async Task<Person> GetPerson(string personGroupId, string personId)
        {
            return null;
        }

        public async Task<IEnumerable<Person>> ListPersonsInGroup(string personGroupId)
        {
            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_appSettings.FaceApiEndpoint);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.FaceApiKey);

            //construct full endpoint uri
            var apiUri = $"{_appSettings.FaceApiEndpoint}/persongroups/{personGroupId}/persons";

            //make request
            var responseMessage = await httpClient.GetAsync(apiUri);

            //return object if sucess or null if not
            if (responseMessage.IsSuccessStatusCode)
            {
                //cast to array of items
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                var responseArray = JArray.Parse(responseString);
                var items = new List<Person>();
                foreach (var response in responseArray)
                {
                    var item = JsonConvert.DeserializeObject<Person>(response.ToString());
                    items.Add(item);
                }

                return items;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> DeletePerson(string personGroupId, string personId)
        {
            return null;
        }
    }
}
