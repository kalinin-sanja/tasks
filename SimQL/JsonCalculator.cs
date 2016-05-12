using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace SimQLTask
{
    public static class JsonCalculator
    {
        public static List<string> Calculate(string input)
        {
            var result = new List<string>();
            JObject jObject = JObject.Parse(input);
            var data = jObject["data"];
            var queries = jObject["queries"].ToObject<List<string>>();
            foreach (var query in queries)
            {
                List<JToken> tokens = new List<JToken> {data};
                var func = query.Substring(0, 3);
                var path = query.Substring(4, query.Length - 5);
                foreach (var index in path.Split('.'))
                {
                    tokens = tokens.SelectMany(t => t[index]).ToList();
                }
            }
            return result;
        }
    }

    [TestFixture]
    public class JsonCalculator_Should
    {
        [Test]
        public void Parse_Sample()
        {
            JsonCalculator.Calculate(
@"{
    'data': {'a':{'x':3.14, 'b':[{'c':15}, {'c':9}]}, 'z':[2.65, 35]},
    'queries': [
        'sum(a.b.c)',
        'min(z)',
        'max(a.x)'
    ]
}");

        }
    }
}