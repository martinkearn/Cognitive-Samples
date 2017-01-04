using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

public static void Run(Stream original, Stream thumb, TraceWriter log)
{
    int width = 320;
    int height = 320;
    bool smartCropping = true;
    string _apiKey = "382f5abd65f74494935027f65a41a4bc";
    string _apiUrlBase = "https://api.projectoxford.ai/vision/v1.0/generateThumbnail";
    
    using (var httpClient = new HttpClient())
    {
        httpClient.BaseAddress = new Uri(_apiUrlBase);
        httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);
        using (HttpContent content = new StreamContent(original))
        {
            //get response
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/octet-stream");
            var uri = $"{_apiUrlBase}?width={width}&height={height}&smartCropping={smartCropping.ToString()}";
            var response = httpClient.PostAsync(uri, content).Result;
            var responseBytes = response.Content.ReadAsByteArrayAsync().Result;
            
            //write to output thumb
            thumb.Write(responseBytes, 0, responseBytes.Length);
        }
    }
}
