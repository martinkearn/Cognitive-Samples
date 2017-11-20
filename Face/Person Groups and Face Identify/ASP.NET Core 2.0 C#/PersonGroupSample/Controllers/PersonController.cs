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
using System.IO;

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
            return RedirectToAction("Index", "PersonGroup");
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

        // GET: Person/AddFace/5
        public async Task<ActionResult> AddFace(string id, string personGroupId)
        {
            var face = new Face()
            {
                personGroupId = personGroupId,
                personId = id,
                targetFace = string.Empty,
                userData = string.Empty,
                faceImage = null
            };

            return View(face);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddFace(IFormCollection collection)
        {
            try
            {
                var face = CastFormCollectionToFace(collection);

                // Get image byte array from request form and add it to object
                byte[] imageBytes = null;
                if (Request.Form.Files.Count > 0)
                {
                    var formFile = Request.Form.Files[0];
                    using (var fileStream = formFile.OpenReadStream())
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        imageBytes = ms.ToArray();
                    }
                }
                face.faceImage = imageBytes;

                var result = await _personRep.AddPersonFace(face.faceImage, face.personGroupId, face.personId, face.userData, face.targetFace);

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
        public async Task<ActionResult> Delete(string id, string personGroupId)
        {
            var person = await _personRep.GetPerson(personGroupId, id);

            var vm = new PersonCreate()
            {
                PersonGroupId = personGroupId,
                Person = person
            };

            return View(vm);
        }

        // POST: Person/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                var personGroupId = collection["personGroupId"];

                var result = await _personRep.DeletePerson(personGroupId, id);

                return RedirectToAction("Index", "PersonGroup");
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

        private Face CastFormCollectionToFace(IFormCollection collection)
        {
            return new Face()
            {
                personGroupId = collection["personGroupId"],
                personId = collection["personId"],
                userData = collection["userData"],
                targetFace = collection["targetFace"]
            };
        }
    }
}