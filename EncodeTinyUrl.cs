using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JasonMrX.MyTinyUrl.Common;

namespace JasonMrX.MyTinyUrl
{
    public static class EncodeTinyUrl
    {
        private static Random rand = new Random();

        public static int GetRandomInt(int max) 
        {
            return rand.Next(max);
        }

        [FunctionName("EncodeTinyUrl")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var tinyUrlKey = rand.Next(100000).ToString();
            TableStorageAccessor.Instance.Insert(rand.Next(100000).ToString(), name);

            return name != null
                ? (ActionResult)new OkObjectResult($"tinyUrlKey: {tinyUrlKey}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
