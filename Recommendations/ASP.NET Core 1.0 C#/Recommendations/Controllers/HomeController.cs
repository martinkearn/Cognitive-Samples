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

namespace Recommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _environment;

        public HomeController(IHostingEnvironment env)
        {
            _environment = env;
        }

        public IActionResult Index()
        {
            //get store data
            var books = new List<Book>();
            var rootPath = _environment.ContentRootPath;
            var storeFilePath = rootPath + "/bookscatalog.txt";
            using (var fileStream = new FileStream(storeFilePath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var cells = line.Split(',');
                        var book = new Book()
                        {
                            Id = cells[0],
                            Title = cells[1],
                            Type = cells[2],
                            Author = cells[3].Substring(cells[3].IndexOf('=')+1),
                            Publisher = cells[4].Substring(cells[4].IndexOf('=') + 1),
                            Year = cells[5].Substring(cells[5].IndexOf('=') + 1)
                        };
                        books.Add(book);
                    }
                }
            }

            //render view
            return View(books);
        }

        public IActionResult Book(string id)
        {
            var book = new Book() {
                Id = id,
                Title = string.Empty,
                Type = string.Empty,
                Author = string.Empty,
                Publisher = string.Empty,
                Year = string.Empty
            };
            return View(book);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
