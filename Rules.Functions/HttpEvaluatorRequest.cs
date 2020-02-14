using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Rules.Functions
{
    public class HttpEvaluatorRequest
    {
        public string Rule { get; set; }
        public JsonElement Input { get; set; }
    }
}
