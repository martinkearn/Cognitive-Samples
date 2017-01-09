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

            var _apiKey = "48905f53e07a46138cc413cd04efb325";

            //construct API Uri
            var parameters = new Dictionary<string, string> {
                { "itemIds", id},
                { "numberOfResults", "5" },
                { "minimalScore", "0" },
                { "includeMetadata", "1" },
                { "buildId", "1600480" },
            };
            var baseApiUrl = "https://westus.api.cognitive.microsoft.com/recommendations/v4.0/models/61b5f30d-de8a-4a9c-b026-058081095ef9/recommend/item";
            var apiUri = QueryHelpers.AddQueryString(baseApiUrl, parameters);
            
            //get recomendations
            var recomendedItems = new RecommendedItems();
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(baseApiUrl);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);

                //make request
                var response = await httpClient.GetAsync(apiUri);

                //read response and parse to object
                var responseContent = await response.Content.ReadAsStringAsync();
                recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
            }

            var vm = new HomeBookViewModel()
            {
                Book = book,
                RecommendedItems = recomendedItems
            };

            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
