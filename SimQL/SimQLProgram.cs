using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimQLTask
{
	class SimQLProgram
	{
		static void Main(string[] args)
		{
			var json = Console.In.ReadToEnd();
			foreach (var result in ExecuteQueries(json))
				Console.WriteLine(result);
		}



		public static IEnumerable<string> ExecuteQueries(string json)
		{
			//Вызов object.ToSting в случае числа использует текущие настройки глобализации, (нампример 1.0 1,1)
			// потому для этого метода переключаемся в Invariant
			var prevCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			var jObject = JObject.Parse(json);
			var data = jObject["data"].ToString();
			var queries = jObject["queries"].ToObject<string[]>();

			var flatternData = JsonHelper.DeserializeAndFlatten(data);
			var executeQueries = new List<string>();
			foreach (var query in queries)
			{
				var funcInfo = QuerryFunctions.Parse(query);
				var filteredData = flatternData.Where(x => x.Path == funcInfo.Args);
				try
				{
					switch (funcInfo.Name)
					{
						case "sum":
							executeQueries.Add($"{query} = {filteredData.Sum(x => double.Parse(x.Value))}");
							break;
						case "min":
							executeQueries.Add($"{query} = {filteredData.Min(x => double.Parse(x.Value))}");
							break;
						case "max":
							executeQueries.Add($"{query} = {filteredData.Max(x => double.Parse(x.Value))}");
							break;
					}
				}
				catch (FormatException)
				{
					executeQueries.Add($"{query} = {0}");
				}

			}
			
			Thread.CurrentThread.CurrentCulture = prevCulture;

			return executeQueries;
		}




	}
}
