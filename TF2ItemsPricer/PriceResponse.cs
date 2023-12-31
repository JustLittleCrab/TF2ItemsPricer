﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer
{
    public class PriceResponse : ItemPrice
    {
        
        [JsonProperty("updatedAt")]
        private readonly DateTime _updatedAt;
        [JsonProperty]
        public readonly int? buyKeyHalfScrap;
        [JsonProperty]
        public readonly int? sellKeyHalfScrap;
        [JsonIgnore]
        public DateTime updatedAt => _updatedAt.ToLocalTime();

    }
}
