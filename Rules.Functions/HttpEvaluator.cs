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
    public static class HttpEvaluator
    {
        private static string code = "function run(input) { const z = (input.a + input.b); return { result : z }; }";

        [FunctionName("HttpEvaluator")]
        public static async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger logger)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var request = JsonSerializer.Deserialize<HttpEvaluatorRequest>(requestBody);
            
            var engine = new Engine(logger);

            var result = engine.Evaluate(code, request.Input);

            var json = result.GetRawText();

            return new ContentResult() { Content = json, ContentType = "text/json" };
        }
    }
}
