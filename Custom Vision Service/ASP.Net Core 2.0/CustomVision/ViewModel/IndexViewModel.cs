using CustomVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVision.ViewModel
{
    public class IndexViewModel
    {
        public PredictionResult PredictionResult { get; set; }

        public string Image { get; set; }
    }
}
