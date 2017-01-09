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

        public IActionResult Book(string id)
        {
            var book = _books.Where(o => o.Id == id).FirstOrDefault();

            return View(book);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
