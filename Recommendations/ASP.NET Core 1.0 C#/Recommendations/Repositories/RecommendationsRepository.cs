using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommendations.Interfaces;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Recommendations.Repositories
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private readonly AppSettings _appSettings;
        private string _baseItemToItemApiUrl;

        public RecommendationsRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            _baseItemToItemApiUrl = _appSettings.RecommendationsApiBaseUrl.Replace("MODELID", _appSettings.RecommendationsApiModelId);
        }

        public async Task<RecommendedItems> GetRecommendedItems(string id, string numberOfResults, string minimalScore)
        {
            var responseContent = await CallRecomendationsApi(id, numberOfResults, minimalScore, _appSettings.RecommendationsApiRecommendationsBuildId);
            var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
            return recomendedItems;
        }

        public async Task<RecommendedItems> GetFBTItems(string id, string numberOfResults, string minimalScore)
        {
            var responseContent = await CallRecomendationsApi(id, numberOfResults, minimalScore, _appSettings.RecommendationsApiFBTBuildId);
            var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
            return recomendedItems;
        }

        private async Task<string> CallRecomendationsApi(string id, string numberOfResults, string minimalScore, string buildId)
        {
            //construct API Uri
            var parameters = new Dictionary<string, string> {
                { "itemIds", id},
                { "numberOfResults", numberOfResults },
                { "minimalScore", minimalScore },
                { "includeMetadata", "1" },
                { "buildId", buildId },
            };
            var apiUri = QueryHelpers.AddQueryString(_baseItemToItemApiUrl, parameters);

            //get item to item recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.RecommendationsApiKey);

                //make request
                var response = await httpClient.GetAsync(apiUri);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return responseContent;
        }
    }
}
