using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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

        
    }
}