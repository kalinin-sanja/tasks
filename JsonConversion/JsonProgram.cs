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
            
		    var wareHouseV2 = JsonConvert.DeserializeObject<WareHouseV2>(json);
		    var wareHouseV3 = WareHouseConverter.Convert(wareHouseV2);
		    var result = JsonConvert.SerializeObject(wareHouseV3);
			//...
//			var v3 = "{ 'version':'3', 'products': 'TODO' }";
			Console.Write(result);
		}
	}
}
