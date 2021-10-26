using System;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aletheia.Service;

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
                throw new Exception("Request to Aletheia failed with code " + resp.StatusCode.ToString());
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
    }
}