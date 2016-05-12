using Newtonsoft.Json.Linq;
using System;
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

            var wareHouseV2 = ParseToWareHouseV2(json);
		    var wareHouseV3 = WareHouseConverter.Convert(wareHouseV2);
		    var result = JsonConvert.SerializeObject(wareHouseV3);
			//...
//			var v3 = "{ 'version':'3', 'products': 'TODO' }";
			Console.Write(result);
		}

	    public static WareHouseV2 ParseToWareHouseV2(string json)
	    {
	        return JsonConvert.DeserializeObject<WareHouseV2>(json.Replace("'", ""));
	    }

//        public class JsonDoubleConverter : JsonConverter
//        {
//            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//            {
//                throw new NotImplementedException();
//            }
//
//            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//            {
//                throw new NotImplementedException();
//            }
//
//            public override bool CanConvert(Type objectType)
//            {
//                return 
//            }
//        }
	}
}
