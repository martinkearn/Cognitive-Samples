using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Interfaces
{
    public interface IPersonRep
    {
        Task<string> CreatePerson(string personGroupId, Person person);

        Task<string> AddPersonFace(byte[] faceImage, string personGroupId, string personId, string userData, string targetFace);

        Task<Person> GetPerson(string personGroupId, string personId);

        Task<IEnumerable<Person>> ListPersonsInGroup(string personGroupId);

        Task<string> DeletePerson(string personGroupId, string personId);
    }
}
