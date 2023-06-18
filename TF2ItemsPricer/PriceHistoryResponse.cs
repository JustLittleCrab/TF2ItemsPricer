using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer
{
    public class PriceHistoryResponse
    {
        public PriceHistoryResponse() { }
        public PriceHistoryResponse(IList<ItemPrice> history, Meta meta)
        {
            History = new ReadOnlyCollection<ItemPrice>(history);
            this.meta = meta;
        }

        [JsonProperty("items")]
        public readonly ReadOnlyCollection<ItemPrice> History;

        [JsonProperty]
        public readonly Meta meta;


        public class Meta
        {
            [JsonProperty]
            public readonly int totalItems;

            [JsonProperty]
            public readonly int itemCount;

            [JsonProperty]
            public readonly int itemsPerPage;

            [JsonProperty]
            public readonly int totalPages;

            [JsonProperty]
            public readonly int currentPage;
        }
    }
}
