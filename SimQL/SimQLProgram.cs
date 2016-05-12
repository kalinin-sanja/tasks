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
				var funcInfo = QueryFunc.Parse(query);


				var filteredData = flatternData
					.Where(x => x.Path == funcInfo.Selector)
					.Select(x =>
					{
						double res;
						return double.TryParse(x.Value,NumberStyles.Any,CultureInfo.InvariantCulture,out  res ) ? res : 0;
					});

				if (funcInfo.From.HasValue)
					filteredData = filteredData.Where(x => x >= funcInfo.From.Value);

				if (funcInfo.Top.HasValue)
					filteredData = filteredData.OrderBy(x => x).Take(funcInfo.Top.Value);


				switch (funcInfo.Name)
				{
					case "sum":
						executeQueries.Add($"{query} = {filteredData.Sum().ToString(CultureInfo.InvariantCulture)}");
						break;
					case "min":
						executeQueries.Add($"{query} = {filteredData.Min().ToString(CultureInfo.InvariantCulture)}");
						break;
					case "max":
						executeQueries.Add($"{query} = {filteredData.Max().ToString(CultureInfo.InvariantCulture)}");
						break;
				}


				Thread.CurrentThread.CurrentCulture = prevCulture;



			}



			return executeQueries;
		}




	}

}
