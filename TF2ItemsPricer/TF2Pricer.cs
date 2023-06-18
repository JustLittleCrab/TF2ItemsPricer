using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer
{
    public class TF2Pricer
    {
        private const int __ATTEMPTS_TO_REQUEST = 3;

        private string AccessToken;
        private DateTime LastTokenUpdate;
        private TimeSpan TokenUpdateInterval = TimeSpan.FromMinutes(5);

        private DateTime LastRequest;
        private TimeSpan RequestsInterval = TimeSpan.FromMilliseconds(100);

        private async Task HeartBeat()
        {
            if (LastTokenUpdate + TokenUpdateInterval < DateTime.Now) await Auth();
        }
        private async Task Auth()
        {
            var access = await RequestAccessToken();
            if (access != null)
            {
                AccessToken = access;
                LastTokenUpdate = DateTime.Now;
            }
        }
        private async Task<string> RequestAccessToken()
        {
            var resp = await Request("https://api2.prices.tf/auth/access", "POST");
            try
            {
                return JsonConvert.DeserializeObject<AccessTokenResponse>(resp).accessToken;
            }
            catch (Exception ex)
            {
                return null;
            }


            return resp;
        }


        private int req_depth = 0;
        private async Task<string> Request(string url, string method)
        {
            if (LastRequest + RequestsInterval > DateTime.Now) await Task.Delay(RequestsInterval);
            LastRequest = DateTime.Now;
            bool isAuthReq = "https://api2.prices.tf/auth/access" == url;

            if (!isAuthReq) await HeartBeat();

            // Setup the request.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Timeout = 50000; // Timeout after 50 seconds.
            request.Accept = "application/json";
            if (!isAuthReq)
            {
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer " + AccessToken);
            }






            // Get the response and return it.
            try
            {
                var resp = await request.GetResponseAsync() as HttpWebResponse;
                req_depth = 0;
                return await ReadResponse(resp);
            }
            catch (WebException ex)
            {
                var resp = ex.Response as HttpWebResponse;
                if (resp != null)
                {
                    if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        if (req_depth > __ATTEMPTS_TO_REQUEST)
                        {
                            return "401";
                        }

                        await Auth();
                        
                    }
                    else if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return "404";
                    }
                    
                }
                req_depth++;
                if (__ATTEMPTS_TO_REQUEST > req_depth)
                {
                    await Task.Delay(300);
                    return await Request(url, method);
                }
                else return null;
            }
        }
        private async Task<string> ReadResponse(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    return await sr.ReadToEndAsync();
                }
            }
        }


        /// <summary>
        /// Get price
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public async Task<PriceResponse> GetPrice(SKU sku)
        {
            var url = $"https://api2.prices.tf/prices/{Uri.EscapeDataString(sku.ToString())}";

            var resp = await Request(url, "GET");
            try
            {
                if (resp != null) return JsonConvert.DeserializeObject<PriceResponse>(resp);
                else return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Send a request to update price for item
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public async Task<PriceUpdateResponse> SendPriceUpdateRequest(SKU sku)
        {
            var url = $"https://api2.prices.tf/prices/{Uri.EscapeDataString(sku.ToString())}/refresh";
            var resp = await Request(url, "POST");
            try
            {
                if (resp != null) return JsonConvert.DeserializeObject<PriceUpdateResponse>(resp);
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Request price history for item
        /// </summary>
        /// <param name="sku">sku of the item</param>
        /// <param name="page">page to request</param>
        /// <param name="limit">limit on page. Must not be greater than 100</param>
        /// <param name="order"></param>
        /// <param name="from">Timestamp to get prices from. Leave empty to request all</param>
        /// <returns></returns>
        public async Task<PriceHistoryResponse> GetPriceHistory(SKU sku, int page = 1, int limit = 100, EPriceHistoryOrder order = EPriceHistoryOrder.Ascending, DateTime from = default)
        {
            string url = $"https://api2.prices.tf/history/{Uri.EscapeDataString(sku.ToString())}?page={page}&limit={limit}&order={(order == EPriceHistoryOrder.Ascending ? "ASC" : "DESC")}{(from != default ? $"&from={from.ToUniversalTime()}" : "")}";

            
            var resp = await Request(url, "GET");
            

            try
            {
                if (resp != null) return JsonConvert.DeserializeObject<PriceHistoryResponse>(resp);
                else return null;
            }
            catch(Exception ex) 
            {
                return null;
            }
        }

        /// <summary>
        /// Request all price history
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<PriceHistoryResponse> GetAllPriceHistory(SKU sku, EPriceHistoryOrder order = EPriceHistoryOrder.Ascending)
        {
            List<ItemPrice> prices = new List<ItemPrice>();

            int total_pages = 1; // this value will be updated from meta

            PriceHistoryResponse.Meta last_meta = null;
            for(int page = 1; page < total_pages+1; page++)
            {
                var resp = await GetPriceHistory(sku, page, order: order);
                if (resp != null)
                {
                    last_meta = resp.meta;
                    total_pages = resp.meta.totalPages;
                    prices.AddRange(resp.History);
                }
                
            }
            return new PriceHistoryResponse(prices, last_meta);
        }
    }
}
