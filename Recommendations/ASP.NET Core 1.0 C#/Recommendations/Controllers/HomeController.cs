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
            var book = _books.Where(o => o.Id == id).FirstOrDefault();

            var _apiKey = "48905f53e07a46138cc413cd04efb325";

            //better way: http://stackoverflow.com/questions/29992848/parse-and-modify-a-query-string-in-net-core
            var _apiBaseUri = "https://westus.api.cognitive.microsoft.com/recommendations/v4.0/models/";
            var _apiFullUri = _apiBaseUri
                + "61b5f30d-de8a-4a9c-b026-058081095ef9"
                + "/recommend/item?"
                + "itemIds=" + id + "&"
                + "numberOfResults=" +"10" +"&"
                + "minimalScore=0.5&"
                + "includeMetadata=1&"
                + "buildId=" + "1600480";
            //Model ID: 61b5f30d-de8a-4a9c-b026-058081095ef9 
            //Recommendations build id: 1600480
            //FBT build id: 1600485 
            //Rank build id: 1600486 

            var recomendations = new List<RecommendedBook>();
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_apiBaseUri);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "48905f53e07a46138cc413cd04efb325");

                //make request
                var response = await httpClient.GetAsync(_apiFullUri);

                //read response and parse to object
                var responseContent = await response.Content.ReadAsStringAsync();
                var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
                foreach (var recomendedItem in recomendedItems.recommendedItems)
                {
                    var recomendedBook = _books.Where(o => o.Id == recomendedItem.items.FirstOrDefault().id).FirstOrDefault();
                    recomendations.Add(new RecommendedBook()
                    {
                        Book = recomendedBook,
                        Rating = Math.Round(recomendedItem.rating, 2),
                        Reasoning = recomendedItem.reasoning.FirstOrDefault()
                    });
                }
            }

            var vm = new HomeBookViewModel()
            {
                Book = book,
                Recomendations = recomendations
            };

            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
