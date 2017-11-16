using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Interfaces
{
    public interface IPersonGroupRep
    {
        Task<IEnumerable<PersonGroup>> GetPersonGroups();

        Task<PersonGroup> GetPersonGroup(string personGroupId);

        Task<PersonGroup> CreatePersonGroup(PersonGroup personGroup);

        Task<PersonGroup> UpdatePersonGroup(PersonGroup personGroup);

        Task<PersonGroupTrainingStatus> TrainPersonGroup(string personGroupId);

        Task<string> DeletePersonGroup(string personGroupId);
    }
}
