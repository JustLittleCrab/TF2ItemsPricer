using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TF2ItemsPricer
{
    public class PriceResponse
    {
        [JsonProperty]
        public readonly string sku;
        [JsonProperty]
        public readonly int buyHalfScrap;
        [JsonProperty]
        public readonly int? buyKeys;
        [JsonProperty]
        public readonly int? buyKeyHalfScrap;
        [JsonProperty]
        public readonly int sellHalfScrap;
        [JsonProperty]
        public readonly int? sellKeys;
        [JsonProperty]
        public readonly int? sellKeyHalfScrap;
        [JsonProperty]
        public readonly DateTime createdAt;
        [JsonProperty]
        public readonly DateTime updatedAt;



    }
}
