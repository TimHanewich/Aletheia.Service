using System;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aletheia.Service;
using System.Collections.Generic;

namespace Aletheia.Service.StockData
{
    public static class Extensions
    {
        public static async Task<StockData> GetStockDataAsync(this AletheiaService service, string symbol, bool include_summary, bool include_statistical)
        {
            string url = "https://api.aletheiaapi.com/StockData";
            url = url + "?symbol=" + symbol.Trim().ToUpper() + "&summary=" + include_summary.ToString().ToLower() + "&statistics=" + include_statistical.ToString().ToLower();
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri(url);
            req.Headers.Add("key", service.ApiKey.ToString());

            HttpClient hc = new HttpClient();
            HttpResponseMessage resp = await hc.SendAsync(req);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                string cons = await resp.Content.ReadAsStringAsync();
                throw new Exception("Request to Aletheia for stock '" + symbol.ToUpper().Trim() + "' failed with code " + resp.StatusCode.ToString() + ". Msg: " + cons);
            }

            //Get the body
            string content = await resp.Content.ReadAsStringAsync();
            JObject jo = JObject.Parse(content);

            StockData ToReturn = new StockData();

            //get the summary
            if (include_summary)
            {
                string summaryJSON = jo.Property("Summary").Value.ToString();
                StockSummaryData ssd = JsonConvert.DeserializeObject<StockSummaryData>(summaryJSON);
                ToReturn.SummaryData = ssd;
            }

            //Get the statistical data
            if (include_statistical)
            {
                string statisticsJSON = jo.Property("Statistics").Value.ToString();
                StockStatisticalData ssd = JsonConvert.DeserializeObject<StockStatisticalData>(statisticsJSON);
                ToReturn.StatisticalData = ssd;
            }

            return ToReturn;

        }
    
        public static async Task<StockData> TryGetStockDataAsync(this AletheiaService service, string symbol, bool include_summary, bool include_statistical)
        {
            //Try but return null if it doesn't work.
            StockData ToReturn = null;
            try
            {
                ToReturn = await GetStockDataAsync(service, symbol, include_summary, include_statistical);
            }
            catch
            {

            }
            return ToReturn;
        }

        public static async Task<StockData[]> GetMultipleStockDataAsync(this AletheiaService service, string[] symbols, bool include_summary, bool include_statistical)
        {
            List<Task<StockData>> ToReturn = new List<Task<StockData>>();
            foreach (string s in symbols)
            {
                Task<StockData> ToAdd = GetStockDataAsync(service, s, include_summary, include_statistical);
                ToReturn.Add(ToAdd);
            }

            StockData[] data = await Task.WhenAll(ToReturn);
            return data;
        }
    
        //Will not throw an error if one of the stocks doesn't work.
        public static async Task<StockData[]> TryGetMultipleStockDataAsync(this AletheiaService service, string[] symbols, bool include_summary, bool include_statistical)
        {
            List<Task<StockData>> ToReturn = new List<Task<StockData>>();
            foreach (string s in symbols)
            {
                Task<StockData> ToAdd = TryGetStockDataAsync(service, s, include_summary, include_statistical);
                ToReturn.Add(ToAdd);
            }

            StockData[] data = await Task.WhenAll(ToReturn);

            //Assemble a list of only those that actually have data
            List<StockData> RealToReturn = new List<StockData>();
            foreach (StockData sd in data)
            {
                if (sd != null)
                {
                    RealToReturn.Add(sd);
                }
            }

            return RealToReturn.ToArray();
        }
    }
}