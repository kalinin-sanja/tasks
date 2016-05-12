using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimQLTask
{
	public struct QuerryFunc
	{
		public readonly string Name;
		public readonly string Args;

		public QuerryFunc(string name, string args)
		{
			Name = name;
			Args = args;
		}
	}
	public static class QuerryFunctions
	{
		public static QuerryFunc Parse(string s)
		{
			var parsed = s.Split('(', ')');
			return new QuerryFunc(parsed[0].ToLower(), parsed[1]);
		}

	}

	public struct Token
	{
		public string Path;
		public double Value;

		public Token(string path, double value)
		{
			Path = path;
			Value = value;
		}
	}
	public class JsonHelper
	{
		public static Dictionary<string, object> DeserializeAndFlatten(string json)
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			JToken token = JToken.Parse(json);
			FillDictionaryFromJToken(dict, token, "");
			return dict;
		}

		private static void FillDictionaryFromJToken(Dictionary<string, object> dict, JToken token, string prefix)
		{
			switch (token.Type)
			{
				case JTokenType.Object:
					foreach (JProperty prop in token.Children<JProperty>())
					{
						FillDictionaryFromJToken(dict, prop.Value, Join(prefix, prop.Name));
					}
					break;

				case JTokenType.Array:
					int index = 0;
					foreach (JToken value in token.Children())
					{
						FillDictionaryFromJToken(dict, value, Join(prefix, ""));
						index++;
					}
					break;

				default:
					dict.Add(prefix, ((JValue)token).Value);
					break;
			}
		}

		private static string Join(string prefix, string name)
		{
			return (string.IsNullOrEmpty(prefix) ? name : prefix + "." + name);
		}
	}
	class SimQLProgram
	{
		static void Main(string[] args)
		{
			var json = Console.In.ReadToEnd();
			foreach (var result in ExecuteQueries(json))
				Console.WriteLine(result);
		}



		public static IEnumerable<double> ExecuteQueries(string json)
		{
			var jObject = JObject.Parse(json);
			var data = jObject["data"].ToString();
			var queries = jObject["queries"].ToObject<string[]>();

			var flatternData = JsonHelper.DeserializeAndFlatten(data);
			foreach (var query in queries)
			{
				var funcInfo = QuerryFunctions.Parse(query);
				var filteredData = flatternData.Where(x => x.Key == funcInfo.Args);
				switch (funcInfo.Name)
				{
					case "sum":
						yield return filteredData.Sum(x => double.Parse(x.Value.ToString()));
						break;
					case "min":
						yield return filteredData.Min(x => double.Parse(x.Value.ToString()));
						break;
					case "max":
						yield return filteredData.Max(x => double.Parse(x.Value.ToString()));
						break;
				}
			}


		}




	}
}
