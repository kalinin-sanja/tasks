using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonConversion
{
	class JsonProgram
	{
		static void Main()
		{
			string json = Console.In.ReadToEnd();
//			JObject v2 = JObject.Parse(json);
//		    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		    Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",";
            var wareHouseV2 = ParseToWareHouseV2(json);
		    var wareHouseV3 = WareHouseConverter.Convert(wareHouseV2);
		    var result = JsonConvert.SerializeObject(wareHouseV3);
			//...
//			var v3 = "{ 'version':'3', 'products': 'TODO' }";
			Console.Write(result);
		}

	    public static WareHouseV2 ParseToWareHouseV2(string json)
	    {
            return JsonConvert.DeserializeObject<WareHouseV2>(CorrectInputJson(json));
	    }

	    private static string CorrectInputJson(string json)
	    {
	        var replace = json.Replace("'", "");
	        var result = replace;
            var regex = new Regex(@",\d\d""");
            var matches = regex.Matches(replace);
	        foreach (Match match in matches)
	        {
	            result = result.Remove(match.Index, 1).Insert(match.Index, ".");
	        }
	        return result;
	    }
	}

    internal class FormattedDecimalConverter : JsonConverter
    {
        public static double ParseDouble(string input)
        {
            var value = input.Replace("'", "");
            return double.Parse(value, CultureInfo.InvariantCulture);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ParseDouble((string) reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(double) == objectType;
        }
    }
}
