using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonConversion
{
    public class WareHouseV3
    {
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("products")]
        public List<ProductV3> Products { get; set; }
    }
}