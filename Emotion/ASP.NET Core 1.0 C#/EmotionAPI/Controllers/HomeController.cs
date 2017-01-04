using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Http.Headers;
using EmotionAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNet.Http;
using System.IO;

namespace EmotionAPI.Controllers
{
    public class HomeController : Controller
    {
        //_apiKey: Replace this with your own Project Oxford Emotion API key, please do not use my key. I inlcude it here so you can get up and running quickly but you can get your own key for free at https://www.projectoxford.ai/emotion 
        public const string _apiKey = "1dd1f4e23a5743139399788aa30a7153";

        //_apiUrl: The base URL for the API. Find out what this is for other APIs via the API documentation
        public const string _apiUrl = "https://api.projectoxford.ai/emotion/v1.0/recognize";

        public IActionResult Index()
        {
            return View();
        }

        // GET: Home/FileExample
        public IActionResult FileExample()
        {
            return View();
        }

        // POST: Home/FileExample
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FileExample(IFormFile file)
        {
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

                //setup data object
                HttpContent content = new StreamContent(file.OpenReadStream());
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/octet-stream");

                //make request
                var response = await httpClient.PostAsync(_apiUrl, content);

                //read response and write to view
                var responseContent = await response.Content.ReadAsStringAsync();
                ViewData["Result"] = responseContent;
            }

            return View();
        }

        public async Task<IActionResult> URLExample()
        {
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_apiUrl);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //setup data object
                var dataObject = new URLData()
                {
                    url = "https://oxfordportal.blob.core.windows.net/emotion/recognition1.jpg"
                };

                //setup httpContent object
                var dataJson = JsonConvert.SerializeObject(dataObject);
                HttpContent content = new StringContent(dataJson);
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                //make request
                var response = await httpClient.PostAsync(_apiUrl, content);

                //read response and write to view
                var responseContent = await response.Content.ReadAsStringAsync();
                ViewData["Result"] = responseContent;
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
