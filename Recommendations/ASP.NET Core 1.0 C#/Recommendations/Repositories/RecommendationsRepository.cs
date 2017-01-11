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

        /// <summary>
        /// Helper function to call the Cognitive Recommendations API with an Item-to-Item build
        /// </summary>
        /// <param name="id">ItemId to seed recommendations on</param>
        /// <param name="numberOfResults">How many results to return</param>
        /// <param name="minimalScore">Minimal score for results to be included</param>
        /// <returns>RecomendedItems object - a list of RecommendItems</returns>
        public async Task<RecommendedItems> GetITIItems(string id, string numberOfResults, string minimalScore)
        {
            var responseContent = await CallRecomendationsApi(id, numberOfResults, minimalScore, _appSettings.RecommendationsApiITIBuildId);
            var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
            return recomendedItems;
        }

        /// <summary>
        /// Helper function to call the Cognitive Recommendations API with an Frequently-Bought-Together build
        /// </summary>
        /// <param name="id">ItemId to seed recommendations on</param>
        /// <param name="numberOfResults">How many results to return</param>
        /// <param name="minimalScore">Minimal score for results to be included</param>
        /// <returns>RecomendedItems object - a list of RecommendItems</returns>
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
