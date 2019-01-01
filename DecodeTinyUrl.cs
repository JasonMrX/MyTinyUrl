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
    public static class DecodeTinyUrl
    {
        [FunctionName("DecodeTinyUrl")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DecodeTinyUrl Triggered.");
            string tinyUrlKey = req.Query["tinyUrlKey"];
            string originalUrl = await TableStorageAccessor.Instance.QueryAsync(tinyUrlKey);

            return tinyUrlKey != null
                ? (ActionResult)new OkObjectResult(originalUrl)
                : new BadRequestObjectResult("The http request does not contain a parameter named tinyUrlKey");
        }
    }
}
