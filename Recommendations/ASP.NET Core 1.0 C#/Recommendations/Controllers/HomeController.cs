using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using Recommendations.Models;
using Recommendations.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recommendations.Services;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Recommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly List<Book> _books;

        public HomeController(IHostingEnvironment env)
        {
            _environment = env;
            _books = new BooksService(_environment).GetBooks();
        }

        public IActionResult Index()
        {
            //render view
            return View(_books);
        }

        public async Task<IActionResult> Book(string id)
        {
            //get this book
            var book = _books.Where(o => o.Id == id).FirstOrDefault();

            //get recommended items
            var recomendedItems = await RecomendationsApiService.GetRecommendedItems(id, "5", "0");

            //get fbt items
            var fbtItems = await RecomendationsApiService.GetFBTItems(id, "5", "0");

            //construct view model
            var vm = new HomeBookViewModel()
            {
                Book = book,
                RecommendedItems = recomendedItems,
                FBTItems = fbtItems
            };

            //return view
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
