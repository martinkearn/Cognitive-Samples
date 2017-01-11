using Microsoft.AspNetCore.Mvc;
using Recommendations.Interfaces;
using Recommendations.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBooksRepository _books;
        private readonly IRecommendationsRepository _recommendations;

        public HomeController(IBooksRepository booksRepository, IRecommendationsRepository recommendationsRepository)
        {
            _books = booksRepository;
            _recommendations = recommendationsRepository;
        }

        public IActionResult Index()
        {
            var allBooks = _books.GetBooks();
            return View(allBooks);
        }

        public async Task<IActionResult> Book(string id)
        {
            //get this book
            var book = _books.GetBooks().Where(o => o.Id == id).FirstOrDefault();

            //get ITI and FBT items
            var itiItems = await _recommendations.GetITIItems(id, "5", "0");
            var fbtItems = await _recommendations.GetFBTItems(id, "5", "0");

            //construct view model
            var vm = new HomeBookViewModel()
            {
                Book = book,
                ITIItems = itiItems,
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
