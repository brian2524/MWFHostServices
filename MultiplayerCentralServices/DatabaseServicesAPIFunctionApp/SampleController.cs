using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http;

namespace DatabaseServicesAPIFunctionApp
{
    public class SampleController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SampleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [FunctionName("SampleEndpoint")]
        public async Task<IActionResult> SampleEndpoint(
            [HttpTrigger(AuthorizationLevel.Admin, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            //  Old way that came with this function (NewtonSoft.Json)
            /*string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;*/

            //  New way since NewtonSoft.Json is outdated. Implemented using (System.Text.Json)
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            JsonElement jsonBody = (JsonElement)(JsonSerializer.Deserialize<Object>(requestBody));

            name = name ?? jsonBody.GetProperty("name").ToString();

                                                                         //  HttpClient httpClient = _httpClientFactory.CreateClient();                  Sample creation of an HttpClient to use for calling APIs like our HostSerivces APIc

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
