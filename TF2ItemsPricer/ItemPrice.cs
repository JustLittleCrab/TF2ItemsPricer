using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer
{
    public class ItemPrice
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
        [JsonProperty("createdAt")]
        private readonly DateTime _createdAt;

        [JsonIgnore]
        public DateTime createdAt => _createdAt.ToLocalTime();
    }
}
