using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer.TF2Price
{
    public class AccessTokenResponse
    {
        [JsonProperty]
        public readonly string accessToken;
    }
}
