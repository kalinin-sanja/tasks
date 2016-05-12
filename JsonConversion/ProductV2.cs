using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonConversion
{
    public class ProductV2
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("size")]
        public List<int> Size { get; set; }
    }
}