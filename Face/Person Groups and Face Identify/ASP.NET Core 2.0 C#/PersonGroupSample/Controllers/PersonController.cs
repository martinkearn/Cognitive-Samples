using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonGroupSample.ViewModels;
using PersonGroupSample.Models;
using PersonGroupSample.Interfaces;
using Microsoft.AspNetCore.Routing;

namespace PersonGroupSample.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRep _personRep;

        public PersonController(IPersonRep personRep)
        {
            _personRep = personRep;
        }

        // GET: Person
        public ActionResult Index()
        {
            return View();
        }

        // GET: Person/Details/5
        public async Task<ActionResult> Details(string id, string personGroupId)
        {
            var person = await _personRep.GetPerson(personGroupId, id);

            var vm = new PersonCreate()
            {
                PersonGroupId = personGroupId,
                Person = person
            };

            return View(vm);
        }

        // GET: Person/Create
        public ActionResult Create(string personGroupId)
        {
            var vm = new PersonCreate()
            {
                PersonGroupId = personGroupId,
                Person = new Person()
            };

            return View(vm);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var person = CastFormCollection(collection);
                var personGroupId = collection["personGroupId"];

                var result = await _personRep.CreatePerson(personGroupId, person);

                return RedirectToAction("Index", "PersonGroup");
            }
            catch
            {
                return View();
            }
        }

        // GET: Person/Edit/5
        public async Task<ActionResult> Edit(string id, string personGroupId)
        {
            var person = await _personRep.GetPerson(personGroupId, id);

            var vm = new PersonCreate()
            {
                PersonGroupId = personGroupId,
                Person = person
            };

            return View(vm);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                var person = CastFormCollection(collection);
                var personGroupId = collection["personGroupId"];

                var result = await _personRep.UpdatePerson(personGroupId, person);

                return RedirectToAction("Index", "PersonGroup");
            }
            catch
            {
                return View();
            }
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Person/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Person CastFormCollection(IFormCollection collection)
        {
            return new Person()
            {
                personId = collection["Person.personId"],
                name = collection["Person.name"],
                userData = collection["Person.userData"]
            };
        }
    }
}