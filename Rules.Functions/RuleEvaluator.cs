using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Rules.Functions
{
    public static class RuleEvaluator
    {
        [FunctionName("RuleEvaluator")]
        public static async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger logger)
        {
            // Read request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<RuleEvaluatorRequest>(requestBody);

            // Download rule code
            var storage = new Storage();
            var code = await storage.DownloadBlobTextAsync("rules", request.Rule);

            // Evaluate code through engine
            var engine = new Engine(logger);
            var result = engine.Evaluate(code, request.Input);

            // Return result
            var json = result.GetRawText();
            return new ContentResult() { Content = json, ContentType = "text/json" };
        }
    }
}
