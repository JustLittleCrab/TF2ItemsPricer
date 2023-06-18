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
        [JsonProperty("sku")]
        public readonly string _sku;
        [JsonProperty]
        public readonly int buyHalfScrap;
        [JsonProperty]
        public readonly int? buyKeys;
        [JsonProperty]
        public readonly int sellHalfScrap;
        [JsonProperty]
        public readonly int? sellKeys;
        [JsonProperty("createdAt")]
        private readonly DateTime _createdAt;

        [JsonIgnore]
        public SKU sku => SKU.Parse(_sku);
        [JsonIgnore]
        public DateTime createdAt => _createdAt.ToLocalTime();
    }
}
