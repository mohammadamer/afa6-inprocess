using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PnP.Core.Services;

namespace Company.Function
{
    public class HttpTrigger1
    {
        private PnPContext ctx;
        public HttpTrigger1(IPnPContextFactory factory){
            ctx = factory.Create(new Uri("https://ateagreen.sharepoint.com/sites/CMP-RolandsTest-Comm"));
        }
        [FunctionName("HttpTrigger1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var web = await ctx.Web.GetAsync(w => w.Title);
            var responseMessage = web.Title;

            return new OkObjectResult(responseMessage);
        }
    }
}
