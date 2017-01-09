using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Models
{
    public class RecommendedBook
    {
        public Book Book { get; set; }
        public string Reasoning { get; set; }
        public double Rating { get; set; }
    }
}
