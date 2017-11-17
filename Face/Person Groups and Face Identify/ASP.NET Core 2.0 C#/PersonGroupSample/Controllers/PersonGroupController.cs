using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonGroupSample.Interfaces;
using PersonGroupSample.Models;

namespace PersonGroupSample.Controllers
{
    public class PersonGroupController : Controller
    {

        private readonly IPersonGroupRep _personGroupRep;

        public PersonGroupController(IPersonGroupRep personGroupRep)
        {
            _personGroupRep = personGroupRep;
        }


        // GET: PersonGroup
        public async Task<ActionResult> Index()
        {
            var personGroups = await _personGroupRep.GetPersonGroups();

            return View(personGroups);
        }

        // GET: PersonGroup/Details/5
        public async Task<ActionResult> Details(string id)
        {
            //get person group
            var personGroup = await _personGroupRep.GetPersonGroup(id);

            //get training status
            var personGroupTrainingStatus = await _personGroupRep.GetPersonGroupTrainingJobStatus(id);

            //get people
            //var peopleInGroup = await _personGro

            return View(personGroup);
        }

        // GET: PersonGroup/Train/5
        public async Task<ActionResult> Train(string id)
        {
            var trainingGroupStatus = await _personGroupRep.GetPersonGroupTrainingJobStatus(id); 
            return View(trainingGroupStatus);
        }

        // POST: PersonGroup/Train/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Train(string id, IFormCollection collection)
        {
            try
            {
                var trainingGroupStatus = await _personGroupRep.CreatePersonGroupTrainingJob(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var personGroup = CastFormCollection(collection);

                var result = await _personGroupRep.CreatePersonGroup(personGroup);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonGroup/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var personGroup = await _personGroupRep.GetPersonGroup(id);

            return View(personGroup);
        }

        // POST: PersonGroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                var personGroup = CastFormCollection(collection);

                var updatedPersonGroup = await _personGroupRep.UpdatePersonGroup(personGroup);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonGroup/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var personGroup = await _personGroupRep.GetPersonGroup(id);

            return View(personGroup);
        }

        // POST: PersonGroup/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                var response = await _personGroupRep.DeletePersonGroup(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private PersonGroup CastFormCollection(IFormCollection collection)
        {
            return new PersonGroup()
            {
                personGroupId = collection["personGroupId"],
                name = collection["name"],
                userData = collection["userData"]
            };
        }
    }
}