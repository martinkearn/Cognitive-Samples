using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;

namespace Thumbnails.Controllers
{
    public class HomeController : Controller
    {
        //_apiKey: Replace this with your own Cognitive Services Computer Vision API key, please do not use my key. I include it here so you can get up and running quickly but you can get your own key for free at https://www.microsoft.com/cognitive-services/en-us/computer-vision-api
        public const string _apiKey = "382f5abd65f74494935027f65a41a4bc";

        //_apiUrl: The base URL for the API. Find out what this is for other APIs via the API documentation
        public const string _apiUrlBase = "https://api.projectoxford.ai/vision/v1.0/generateThumbnail";

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
            //put the original file in the view data so we can compare it
            ViewData["originalImage"] = FileToImgSrcString(file);
            
            //get input data from form
            var width = (string.IsNullOrEmpty(Request.Form["width"].ToString())) ?
                "100" :
                Request.Form["width"].ToString();

            var height = (string.IsNullOrEmpty(Request.Form["height"].ToString())) ?
                "100" :
                Request.Form["height"].ToString();

            var smartcropping = (string.IsNullOrEmpty(Request.Form["smartcropping"].ToString())) ?
                "false" :
                "true";

            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_apiUrlBase);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);

                //setup data object
                HttpContent content = new StreamContent(file.OpenReadStream());
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/octet-stream");

                // Request parameters
                var uri = $"{_apiUrlBase}?width={width}&height={height}&smartCropping={smartcropping}";

                //make request
                var response = await httpClient.PostAsync(uri, content);

                //read response and write to view data
                var responseContent = await response.Content.ReadAsByteArrayAsync();
                ViewData["thumbnailImage"] = BytesToSrcString(responseContent);
            }

            return View();
        }

        // GET: Home/UrlExample
        public IActionResult URLExample()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> URLExample(string width = "5", string height = "5", bool smartcropping = true)
        {
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_apiUrlBase);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);

                //setup httpContent object
                var imageUrl = "https://oxfordportal.blob.core.windows.net/emotion/recognition1.jpg";
                HttpContent content = new StringContent($"{{\"url\":\"{imageUrl}\"}}");
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                // Request parameters
                var uri = $"{_apiUrlBase}?width={width}&height={height}&smartCropping={smartcropping}";

                //make request
                var response = await httpClient.PostAsync(uri, content);

                //read response and write to view
                var responseContent = await response.Content.ReadAsByteArrayAsync();
                ViewData["thumbnailImage"] = BytesToSrcString(responseContent);
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }


        private string FileToImgSrcString(IFormFile file)
        {
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }
            return BytesToSrcString(fileBytes);
        }

        private string BytesToSrcString(byte[] bytes) => "data:image/jpg;base64," + Convert.ToBase64String(bytes);
    }
}
