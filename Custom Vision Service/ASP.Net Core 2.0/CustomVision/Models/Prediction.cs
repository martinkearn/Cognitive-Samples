using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVision.Models
{
    public class Prediction
    {
        public string TagId { get; set; }
        public string Tag { get; set; }
        public double Probability { get; set; }
    }
}
