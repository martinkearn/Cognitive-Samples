using Microsoft.Extensions.Options;
using PersonGroupSample.Interfaces;
using PersonGroupSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Repositories
{
    public class PersonRep : IPersonRep
    {
        private readonly AppSettings _appSettings;

        public PersonRep(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

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
