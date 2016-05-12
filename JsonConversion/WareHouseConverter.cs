using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonConversion
{
    public class WareHouseConverter
    {
        public static WareHouseV3 Convert(WareHouseV2 wareHouseV2)
        {
            var result = new WareHouseV3
            {
                Version = 3,
                Products = wareHouseV2.Products.Select(kv => new ProductV3
                {
                    Id = kv.Key,
                    Name = kv.Value.Name,
                    Price = kv.Value.Price,
                    Count = kv.Value.Count
                }).ToList()
            };
            return result;
        }
    }

    [TestFixture]
    public class WareHouseConverter_Should
    {
        [Test]
        public void Convert_OnlyProducts()
        {
            var oldJson = File.ReadAllText("1.v2.json");
            var newJson = File.ReadAllText("1.v3.json");

            var wareHouseV2 = JsonConvert.DeserializeObject<WareHouseV2>(oldJson);
            var wareHouseV3 = JsonConvert.DeserializeObject<WareHouseV3>(newJson);
            var wareHouseV3Converted = WareHouseConverter.Convert(wareHouseV2);
//            var result = JsonConvert.SerializeObject(wareHouseV3);
            wareHouseV3.ShouldBeEquivalentTo(wareHouseV3Converted);
//            Assert.Equ(wareHouseV3, wareHouseV3Converted);
        }

        [Test]
        public void Convert_OnlyProducts_Write()
        {
            var oldJson = File.ReadAllText("1.v2.json");
            var newJson = File.ReadAllText("1.v3.json");

            var wareHouseV2 = JsonConvert.DeserializeObject<WareHouseV2>(oldJson);
//            var wareHouseV3 = JsonConvert.DeserializeObject<WareHouseV3>(newJson);
            var wareHouseV3Converted = WareHouseConverter.Convert(wareHouseV2);
            var result = JsonConvert.SerializeObject(wareHouseV3Converted);
            Assert.NotNull(result);
//            wareHouseV3.ShouldBeEquivalentTo(wareHouseV3Converted);
            //            Assert.Equ(wareHouseV3, wareHouseV3Converted);
        }
    }
}