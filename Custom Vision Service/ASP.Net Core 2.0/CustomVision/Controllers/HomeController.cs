using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomVision.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CustomVision.ViewModel;

namespace CustomVision.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(null);
        }

        // POST: Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IFormCollection collection)
        {
            var projectId = "b9d89c0c-618e-4698-b0f0-db85d7ddb888";
            var predictionKey = "f23b13aeac6943939afeef1d4517b7ba";

            var file = Request.Form.Files.FirstOrDefault();
            byte[] fileBytes = null;
            using (var fileStream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }

            //send to api to get results
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Prediction-key", predictionKey);

            // Request parameters
            var uri = $"https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/{projectId}/image";

            // Request body
            var content = new ByteArrayContent(fileBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var responseMessage = await client.PostAsync(uri, content);

            if (!responseMessage.IsSuccessStatusCode)
            {
                ViewData["message"] = responseMessage.ReasonPhrase;
                return View(null);
            }

            //deserialise json to object
            var responseString = await responseMessage.Content.ReadAsStringAsync();
            var predictionResult = JsonConvert.DeserializeObject<PredictionResult>(responseString);

            //prepare image as base64 bytes string
            var imageBase64 = Convert.ToBase64String(fileBytes);
            var imageString = $"data:image/png;base64,{imageBase64}";

            //create view model
            var vm = new IndexViewModel()
            {
                PredictionResult = predictionResult,
                Image = imageString
            };

            return View(vm);

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
