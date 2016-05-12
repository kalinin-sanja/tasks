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
		public string Value;

		public Token(string path, string value)
		{
			Path = path;
			Value = value;
		}
	}

	public class JsonHelper
	{
		public static List<Token> DeserializeAndFlatten(string json)
		{
			List<Token> entries = new List<Token>();
			JToken token = JToken.Parse(json);
			FillDictionaryFromJToken(entries, token, "");
			return entries;
		}

		private static void FillDictionaryFromJToken(List<Token> entries, JToken token, string prefix)
		{
			switch (token.Type)
			{
				case JTokenType.Object:
					foreach (JProperty prop in token.Children<JProperty>())
					{
						FillDictionaryFromJToken(entries, prop.Value, Join(prefix, prop.Name));
					}
					break;

				case JTokenType.Array:
					int index = 0;
					foreach (JToken value in token.Children())
					{
						FillDictionaryFromJToken(entries, value, Join(prefix, ""));
						index++;
					}
					break;

				default:
					entries.Add(new Token(prefix, ((JValue)token).Value.ToString()));
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
			var executeQueries = new List<double>();
			foreach (var query in queries)
			{
				var funcInfo = QuerryFunctions.Parse(query);
				var filteredData = flatternData.Where(x => x.Path == funcInfo.Args);
				try
				{
					switch (funcInfo.Name)
					{
						case "sum":
							executeQueries.Add(filteredData.Sum(x => double.Parse(x.Value)));
							break;
						case "min":
							executeQueries.Add(filteredData.Min(x => double.Parse(x.Value)));
							break;
						case "max":
							executeQueries.Add(filteredData.Max(x => double.Parse(x.Value)));
							break;
					}
				}
				catch (FormatException)
				{
					executeQueries.Add(0);
				}

			}


			return executeQueries;
		}




	}

}
