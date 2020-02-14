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
            var engine = new Jint.Engine(options =>
            {
                options
                .LimitRecursion(1)
                .TimeoutInterval(new TimeSpan(0, 0, 3));
            });
            
            engine.SetValue("log", new Action<object>((message) =>
            {
                _logger.LogDebug("Jint Log", message);
            }));

            var jsonParser = new JsonParser(engine);
            var jintInput = jsonParser.Parse(input.ToString());

            engine.Execute(code);

            var result = engine.Invoke("run", jintInput);
            var stringResult = engine.Json.Stringify(result, Jint.Runtime.Arguments.From(result)).ToString();
            return JsonDocument.Parse(stringResult).RootElement;
        }
    }
}
