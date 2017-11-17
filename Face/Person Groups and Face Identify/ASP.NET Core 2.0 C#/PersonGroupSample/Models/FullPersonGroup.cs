using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Models
{
    public class FullPersonGroup
    {
        public PersonGroup PersonGroup { get; set; }
        public PersonGroupTrainingStatus TrainingStatus { get; set; }
        public List<Person> Members { get; set; }
    }
}
