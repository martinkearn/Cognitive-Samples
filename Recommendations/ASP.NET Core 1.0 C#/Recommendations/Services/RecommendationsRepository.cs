using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Recommendations.Interfaces;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Recommendations.Services
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private string _apiKey = "48905f53e07a46138cc413cd04efb325";
        private string _modeldId = "61b5f30d-de8a-4a9c-b026-058081095ef9";
        private string _recommendationBuildId = "1600480";
        private string _fbtBuildId = "1600485";
        private string _baseItemToItemApiUrl;

        public RecommendationsRepository()
        {
            _baseItemToItemApiUrl = string.Format("https://westus.api.cognitive.microsoft.com/recommendations/v4.0/models/{0}/recommend/item", _modeldId);
        }

        public async Task<RecommendedItems> GetRecommendedItems(string id, string numberOfResults, string minimalScore)
        {
            var responseContent = await CallRecomendationsApi(id, numberOfResults, minimalScore, _recommendationBuildId);
            var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
            return recomendedItems;
        }

        public async Task<RecommendedItems> GetFBTItems(string id, string numberOfResults, string minimalScore)
        {
            var responseContent = await CallRecomendationsApi(id, numberOfResults, minimalScore, _fbtBuildId);
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
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);

                //make request
                var response = await httpClient.GetAsync(apiUri);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return responseContent;
        }
    }
}
