using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

//namespace GameInstanceManagementFunctionApp
//{
//    [FunctionName("SpinUp")]
//    public static async Task<IActionResult> Run(
//            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req,
//            ILogger log)
//    {
//        log.LogInformation("SpinUp request processed.");


//        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

//        Object deserialized = JsonSerializer.Deserialize<Object>(requestBody);
//        JsonElement jsonElement = (JsonElement)deserialized;

//        string reqServerIp = jsonElement.GetProperty("ServerIp").ToString();
//        GameInstanceModel reqGameInstance = JsonSerializer.Deserialize<GameInstanceModel>(jsonElement.GetProperty("GameInstanceModel").ToString());


//        //reqServerIp + "api/spin/spinup"

//        return reqServerIp != null
//            ? (ActionResult)new OkObjectResult($"Spun up on {reqServerIp}")
//            : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
//    }
//}
