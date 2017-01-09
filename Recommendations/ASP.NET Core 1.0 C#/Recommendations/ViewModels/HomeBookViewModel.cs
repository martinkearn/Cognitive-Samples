using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.ViewModels
{
    public class HomeBookViewModel
    {
        public Book Book { get; set; }
        public RecommendedItems RecommendedItems { get; set; }
    }
}
