using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Repositories
{
    public class PersonRep
    {
        public async Task<string> CreatePerson(string personGroupId, Person person)
        {
            return null;
        }

        public async Task<string> AddPersonFace(byte[] faceImage, string personGroupId, string personId, string userData, string targetFace)
        {
            return null;
        }

        public async Task<Person> GetPerson(string personGroupId, string personId)
        {
            return null;
        }

        public async Task<IEnumerable<Person>> ListPersonsInGroup(string personGroupId)
        {
            return null;
        }

        public async Task<string> DeletePerson(string personGroupId, string personId)
        {
            return null;
        }
    }
}
