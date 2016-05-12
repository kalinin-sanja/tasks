using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonConversion
{
    public class ProductV3
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? Price { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("dimensions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dimension Dimensions { get; set; }
    }

    public class Dimension
    {
        [JsonProperty("l")]
        public decimal Length { get; set; }
        [JsonProperty("w")]
        public decimal Width { get; set; }
        [JsonProperty("h")]
        public decimal Height { get; set; }

        public static Dimension FromArray(List<decimal> size)
        {
            return new Dimension {Height = size[1], Length = size[2], Width = size[0]};
        }
    }
}