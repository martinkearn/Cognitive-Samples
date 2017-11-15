using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Models
{
    public class PersonGroupTrainingStatus
    {
        public string status { get; set; }
        public string createdDateTime { get; set; }
        public object lastActionDateTime { get; set; }
        public object message { get; set; }
    }
}
