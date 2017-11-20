using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Models
{
    public class Person
    {
        public string personId { get; set; }
        public string name { get; set; }
        public string userData { get; set; }
        public List<string> persistedFaceIds { get; set; }
    }
}
