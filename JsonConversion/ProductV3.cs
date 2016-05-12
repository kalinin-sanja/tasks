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
        public double? Price { get; set; }
        [JsonProperty("count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Count { get; set; }
        [JsonProperty("dimensions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dimension Dimensions { get; set; }
    }

    public class Dimension
    {
        [JsonProperty("l", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? Length { get; set; }
        [JsonProperty("w", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? Width { get; set; }
        [JsonProperty("h", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal? Height { get; set; }

        public static Dimension FromArray(List<decimal> size)
        {
            return new Dimension
            {
                Height = size.Count > 1 ? size[1] : (decimal?) null,
                Length = size.Count > 2 ? size[2] : (decimal?) null,
                Width = size.Count > 0 ? size[0] : (decimal?) null
            };
        }
    }
}