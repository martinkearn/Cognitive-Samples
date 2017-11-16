using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonGroupSample.Models;
using PersonGroupSample.Interfaces;

namespace PersonGroupSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonGroupRep _personGroupRep;

        public HomeController(IPersonGroupRep personGroupRep)
        {
            _personGroupRep = personGroupRep;
        }

        public async Task<IActionResult> Index()
        {
            var name = (new Random((int)DateTime.Now.Ticks)).Next().ToString();
            var personGroup = new PersonGroup() { name = name, personGroupId = name, userData = "No user data" };

            var result = await _personGroupRep.CreatePersonGroup(personGroup);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
