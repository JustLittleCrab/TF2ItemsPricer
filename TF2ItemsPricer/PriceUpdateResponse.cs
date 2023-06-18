using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer
{
    public class PriceUpdateResponse
    {
        [JsonProperty]
        public readonly bool enqueued;
        [JsonProperty]
        public readonly string state;
    }
}
