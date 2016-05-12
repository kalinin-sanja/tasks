using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonConversion
{
    public class WareHouseV2
    {
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("products")]
        public Dictionary<int, ProductV2> Products { get; set; }
    }
}