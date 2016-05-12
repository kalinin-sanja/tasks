using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace JsonConversion
{
    public class ProductV2
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")/*, JsonConverter(typeof(DoubleStringConverter))*/]
        public double? Price { get; set; }
        [JsonProperty("count")]
        public int? Count { get; set; }
        [JsonProperty("size")/*, JsonConverter(typeof(DoubleStringConverter))*/]
        public List<double> Size { get; set; }
    }

    public class DoubleStringConverter : JsonConverter
    {
        public static double ParseDouble(string input)
        {
            var value = input.Replace("'", "");
            return double.Parse(value);
        }
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            return ParseDouble(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}