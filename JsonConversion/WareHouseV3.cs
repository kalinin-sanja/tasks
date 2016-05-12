using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonConversion
{
    public class WareHouseV3
    {
//        public override bool Equals(object obj)
//        {
//            var wh2 = (WareHouseV3)
//            return base.Equals(obj);
//        }

        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("products")]
        public List<ProductV3> Products { get; set; }
    }
}