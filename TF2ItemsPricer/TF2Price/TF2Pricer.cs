using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer.TF2Price
{
    public class TF2Pricer
    {
        private static string ItemPriceURL(SKU sku)
        {
            return $"https://api2.prices.tf/prices/{Uri.EscapeDataString( sku.ToString())}";
        }

        private static string ItemPriceUpdateURL(SKU sku)
        {
            return $"https://api2.prices.tf/prices/{Uri.EscapeDataString(sku.ToString())}/refresh";
        }

        private const string AccessTokenURL = "https://api2.prices.tf/auth/access";

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
            if(access != null)
            {
                this.AccessToken = access;
                this.LastTokenUpdate = DateTime.Now;
            }
        }
        private async Task<string> RequestAccessToken()
        {
            var resp = await Request("https://api2.prices.tf/auth/access", "POST");
            try
            {
                return JsonConvert.DeserializeObject<AccessTokenResponse>(resp).accessToken;
            } catch(Exception ex)
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
            bool isAuthReq = ("https://api2.prices.tf/auth/access" == url);

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
                var resp = (await request.GetResponseAsync()) as HttpWebResponse;
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
                        if(req_depth > 3)
                        {
                            return "401";
                        }

                        req_depth++;
                        await Auth();
                        await Task.Delay(300);
                        return await Request(url, method);
                    }
                    else if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return "404";
                    }

                    return await ReadResponse(resp);
                }
                
                
                return null;
            }
        }
        private async Task< string> ReadResponse(HttpWebResponse response)
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
        /// Get price for item
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public async Task<PriceResponse> GetPrice(SKU sku)
        {
            var url = TF2Pricer.ItemPriceURL(sku);

            var resp = await Request(url, "GET");
            try
            {
                if (resp != null) return JsonConvert.DeserializeObject<PriceResponse>(resp);
                else return null;

            }catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Send a request to update price for item
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public async Task<PriceUpdateResponse> SendUpdatePriceRequest(SKU sku)
        {
            var url = TF2Pricer.ItemPriceUpdateURL(sku);
            var resp = await Request(url, "POST");
            try
            {
                if (resp != null) return JsonConvert.DeserializeObject<PriceUpdateResponse>(resp);
                else return null;
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}
