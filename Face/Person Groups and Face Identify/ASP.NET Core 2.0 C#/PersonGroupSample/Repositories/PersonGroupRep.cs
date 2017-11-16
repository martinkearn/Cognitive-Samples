using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

        public IEnumerable<PersonGroup> GetPersonGroups()
        {
            return null;
        }

        public PersonGroup GetPersonGroup(string personGroupId)
        {
            return null;
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

        public PersonGroupTrainingStatus TrainPersonGroup(string personGroupId)
        {
            return null;
        }

        public string DeletePersonGroup(string personGroupId)
        {
            return null;
        }
    }
}
