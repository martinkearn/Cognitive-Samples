using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Interfaces
{
    public interface IPersonGroupRep
    {
        IEnumerable<PersonGroup> GetPersonGroups();

        PersonGroup GetPersonGroup(string personGroupId);

        Task<PersonGroup> CreatePersonGroup(PersonGroup personGroup);

        PersonGroupTrainingStatus TrainPersonGroup(string personGroupId);

        string DeletePersonGroup(string personGroupId);
    }
}
