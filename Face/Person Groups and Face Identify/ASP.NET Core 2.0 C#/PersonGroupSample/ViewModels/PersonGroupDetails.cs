using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.ViewModels
{
    public class PersonGroupDetails
    {
        public PersonGroup PersonGroup { get; set; }

        public PersonGroupTrainingStatus TrainingStatus { get; set; }

        public List<Person> People { get; set; }
    }
}
