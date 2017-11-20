using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.ViewModels
{
    public class PersonCreate
    {
        public string PersonGroupId { get; set; }

        public Person Person { get; set; }
    }
}
