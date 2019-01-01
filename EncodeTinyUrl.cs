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
using System.Text;
using System.Web;

namespace JasonMrX.MyTinyUrl
{
    public static class EncodeTinyUrl
    {
        private const string ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int DEFAULT_KEY_LENGTH = 6;
        private static readonly Random rand = new Random();

        [FunctionName("EncodeTinyUrl")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("EncodeTinyUrl Triggered.");
            string url = HttpUtility.UrlEncode(req.Query["url"]);
            string tinyUrlKey = await GetTinyUrlKey();
            await TableStorageAccessor.Instance.InsertAsync(tinyUrlKey, url);

            return url != null
                ? (ActionResult)new OkObjectResult($"http://mytinyurl.com/{tinyUrlKey}")
                : new BadRequestObjectResult("The http request does not contain a parameter named url");
        }

        private static async Task<string> GetTinyUrlKey()
        {
            return GetRandKey();
        }

        private static string GetRandKey(int keyLength = DEFAULT_KEY_LENGTH)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < keyLength; i++)
            {
                sb.Append(ALPHABET[rand.Next(62)]);
            }
            return sb.ToString();
        }
    }
}
