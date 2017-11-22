using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVision.Models
{
    public class PredictionResult
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public DateTime Created { get; set; }
        public List<Prediction> Predictions { get; set; }
    }
}
