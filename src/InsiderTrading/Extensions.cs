using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TimHanewich.Toolkit.Web;

namespace Aletheia.Service.InsiderTrading
{
    public static class Extensions
    {
        public static async Task<Entity[]> SearchEntitiesAsync(this AletheiaService service, string term, int top = 20)
        {
            string url = "https://api.aletheiaapi.com/SearchEntities?term=" + term + "&top=" + top.ToString();
            HttpRequestMessage req = service.PrepareHttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri(url);

            HttpClient hc = new HttpClient();
            HttpResponseMessage resp = await hc.SendAsync(req);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                string msg = await resp.Content.ReadAsStringAsync();
                throw new Exception("Entity search failed with code " + resp.StatusCode.ToString() + ". Msg: " + msg);
            }

            //Get content
            string content = await resp.Content.ReadAsStringAsync();
            JArray objs = JArray.Parse(content);

            //Parse
            List<Entity> ToReturn = new List<Entity>();
            foreach (JObject jo in objs)
            {
                Entity e = new Entity();

                //Cik
                try
                {
                    e.Cik = Convert.ToInt32(jo.Property("Cik").Value.ToString());
                }
                catch
                {

                }

                //Name
                try
                {
                    e.Name = jo.Property("Name").Value.ToString();
                }
                catch
                {

                }

                //TradingSymbol
                try
                {
                    e.TradingSymbol = jo.Property("TradingSymbol").Value.ToString();
                }
                catch
                {

                }

                ToReturn.Add(e);
            }

            return ToReturn.ToArray();
        }

        public static async Task<SecurityTransactionHolding[]> LatestTransactions(this AletheiaService service, LatestTransactionsRequest request)
        {

            #region "Construct the URL"


            UrlConstructor urlcon = new UrlConstructor();
            urlcon.Base = "https://api.aletheiaapi.com/LatestTransactions";

            //Issuer
            if (request.Issuer != null)
            {
                if (request.Issuer != "")
                {
                    urlcon.AddQueryParameter(new UrlQueryParameter("issuer", request.Issuer));
                }
            }

            //Owner
            if (request.Owner != null)
            {
                if (request.Owner != "")
                {
                    urlcon.AddQueryParameter(new UrlQueryParameter("owner", request.Owner));
                }
            }

            //Top 
            if (request.Top.HasValue)
            {
                urlcon.AddQueryParameter(new UrlQueryParameter("top", request.Top.Value.ToString()));
            }

            //Before
            if (request.Before.HasValue)
            {
                string DateFormatted = request.Before.Value.Year.ToString("0000") + request.Before.Value.Month.ToString("00") + request.Before.Value.Day.ToString("00");
                urlcon.AddQueryParameter(new UrlQueryParameter("before", DateFormatted));
            }
            
            //Security type
            if (request.SecurityType.HasValue)
            {
                urlcon.AddQueryParameter(new UrlQueryParameter("SecurityType", Convert.ToInt32(request.SecurityType.Value).ToString()));
            }

            //Transaction type
            if (request.TransactionType.HasValue)
            {
                urlcon.AddQueryParameter(new UrlQueryParameter("TransactionType", Convert.ToInt32(request.TransactionType.Value).ToString()));
            }

            //Cascade
            if (request.Cascade.HasValue)
            {
                urlcon.AddQueryParameter(new UrlQueryParameter("cascade", request.Cascade.Value.ToString().ToLower()));
            }

            string URL = urlcon.ToString();

            #endregion
        
            //Make the call
            HttpRequestMessage req = service.PrepareHttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri(URL);
            HttpClient hc = new HttpClient();
            HttpResponseMessage resp = await hc.SendAsync(req);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                string content = await resp.Content.ReadAsStringAsync();
                throw new Exception("Latest Transactions request to Aletheia returned code " + resp.StatusCode.ToString() + ". Msg: " + content);
            }

            //Parse body
            string body = await resp.Content.ReadAsStringAsync();
            SecurityTransactionHolding[] ToReturn = JsonConvert.DeserializeObject<SecurityTransactionHolding[]>(body);
            return ToReturn;
        }
        
    }
}