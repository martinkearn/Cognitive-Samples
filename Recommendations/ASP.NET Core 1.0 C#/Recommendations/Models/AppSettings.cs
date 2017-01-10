using Recommendations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Models
{
    public class AppSettings
    {
        public string RecommendationsApiBaseUrl { get; set; }
        public string RecommendationsApiKey { get; set; }
        public string RecommendationsApiModelId { get; set; }
        public string RecommendationsApiRecommendationsBuildId { get; set; }
        public string RecommendationsApiFBTBuildId { get; set; }
    }
}
