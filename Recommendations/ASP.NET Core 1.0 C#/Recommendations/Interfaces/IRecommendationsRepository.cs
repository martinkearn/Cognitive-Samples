using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IRecommendationsRepository
    {
        Task<RecommendedItems> GetITIItems(string id, string numberOfResults, string minimalScore);
        Task<RecommendedItems> GetFBTItems(string id, string numberOfResults, string minimalScore);
    }
}
