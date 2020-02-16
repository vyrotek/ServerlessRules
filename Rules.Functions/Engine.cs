using Jint.Native.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rules.Functions
{
    public class Engine
    {
        private ILogger _logger;

        public Engine(ILogger logger)
        {
            _logger = logger;
        }

        public JsonElement Evaluate(string code, JsonElement input)
        {
            // Create a JS engine with some sandbox limits
            var engine = new Jint.Engine(options =>
            {
                options
                .LimitRecursion(1)
                .TimeoutInterval(new TimeSpan(0, 0, 3));
            });
            
            var jsonParser = new JsonParser(engine);
            var jintInput = jsonParser.Parse(input.ToString());

            // Go!
            engine.Execute(code);

            // Parse engine result object into json
            var result = engine.Invoke("run", jintInput);
            var stringResult = engine.Json.Stringify(result, Jint.Runtime.Arguments.From(result)).ToString();
            return JsonDocument.Parse(stringResult).RootElement;
        }
    }
}
