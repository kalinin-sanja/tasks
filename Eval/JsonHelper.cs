using System.Collections.Generic;
using System.Globalization;
using Antlr.Runtime;
using Newtonsoft.Json.Linq;

namespace EvalTask
{
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
						FillDictionaryFromJToken(entries, value, Join(prefix,$"{index}"));
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
}