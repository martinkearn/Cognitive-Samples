using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FaceVerify.Models;

namespace FaceVerify.Controllers
{
    public class HomeController : Controller
    {
        public const string _detectApiUrl = "https://api.projectoxford.ai/face/v1.0/detect?";
        public const string _faceApiKey = "077bcd15b2004f3a99e4f947e28d09e7";

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
        public async Task<IActionResult> FileExample(IList<IFormFile> files)
        {
            var faceIds = new List<string>();

            //get face IDs using the Face-Detect function: https://dev.projectoxford.ai/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236
            using (var detectHttpClient = new HttpClient())
            {
                //setup HttpClient
                detectHttpClient.BaseAddress = new Uri(_detectApiUrl);
                detectHttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _faceApiKey);
                detectHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

                // Request parameters
                var detectUri = _detectApiUrl + "returnFaceId=true";

                foreach (var file in files)
                {
                    //setup data object
                    HttpContent content = new StreamContent(file.OpenReadStream());
                    content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/octet-stream");

                    //make request
                    var response = await detectHttpClient.PostAsync(detectUri, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    //deserialise
                    var responseArray = JArray.Parse(responseString);
                    var faces = JsonConvert.DeserializeObject<List<Face>>(responseArray.ToString());
                    faceIds.Add(faces.FirstOrDefault().faceId);
                }


            }


            return View();
        }


    }
}
