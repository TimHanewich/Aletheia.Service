using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimHanewich.Toolkit.Web;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aletheia.Service.EarningsCalls
{
    public static class Extensions
    {
        public static async Task<EarningsCall[]> SearchEarningsCallsAsync(this AletheiaService service, string company, int? year, FiscalPeriod? quarter, int? top)
        {

            #region "Construct the URL"

            UrlConstructor con = new UrlConstructor();
            con.Base = "https://api.aletheiaapi.com/SearchEarningsCalls";

            //Company
            if (company != null)
            {
                if (company != "")
                {
                    con.AddQueryParameter(new UrlQueryParameter("company", company));
                }
            }

            //Year
            if (year.HasValue)
            {
                con.AddQueryParameter(new UrlQueryParameter("year", year.Value.ToString()));
            }

            //Quarter
            if (quarter.HasValue)
            {
                if (quarter.Value == FiscalPeriod.Q1)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q1"));
                }
                else if (quarter.Value == FiscalPeriod.Q2)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q2"));
                }
                else if (quarter.Value == FiscalPeriod.Q3)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q3"));
                }
                else if (quarter.Value == FiscalPeriod.Q4)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q4"));
                }
            }

            //top
            if (top.HasValue)
            {
                con.AddQueryParameter(new UrlQueryParameter("top", top.Value.ToString()));
            }

            #endregion
        
            //Make the call
            HttpRequestMessage req = service.PrepareHttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri(con.ToString());
            HttpClient hc = new HttpClient();
            HttpResponseMessage resp = await hc.SendAsync(req);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                string content = await resp.Content.ReadAsStringAsync();
                throw new Exception("Search Earnings Calls request to Aletheia returned code " + resp.StatusCode.ToString() + ". Msg: " + content);
            }

            //Parse the results
            string body = await resp.Content.ReadAsStringAsync();
            EarningsCall[] calls = JsonConvert.DeserializeObject<EarningsCall[]>(body);
            return calls;

        }
    
        public static async Task<EarningsCall> GetEarningsCallAsync(this AletheiaService service, string company, int? year, FiscalPeriod? quarter, int? begin, int? end, bool? count_remarks)
        {
            #region "Construct the URL"

            UrlConstructor con = new UrlConstructor();
            con.Base = "https://api.aletheiaapi.com/EarningsCall";
            
            //Company
            con.AddQueryParameter(new UrlQueryParameter("company", company));

            //year
            if (year.HasValue)
            {
                con.AddQueryParameter(new UrlQueryParameter("year", year.Value.ToString()));
            }

            //Quarter
            if (quarter.HasValue)
            {
                if (quarter.Value == FiscalPeriod.Q1)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q1"));
                }
                else if (quarter.Value == FiscalPeriod.Q2)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q2"));
                }
                else if (quarter.Value == FiscalPeriod.Q3)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q3"));
                }
                else if (quarter.Value == FiscalPeriod.Q4)
                {
                    con.AddQueryParameter(new UrlQueryParameter("quarter", "q4"));
                }
            }

            //begin
            if (begin.HasValue)
            {
                con.AddQueryParameter(new UrlQueryParameter("begin", begin.Value.ToString()));
            }

            //End
            if (end.HasValue)
            {
                con.AddQueryParameter(new UrlQueryParameter("end", end.Value.ToString()));
            }

            //Count remarks
            if (count_remarks.HasValue)
            {
                con.AddQueryParameter(new UrlQueryParameter("countremarks", count_remarks.Value.ToString().ToLower()));
            }


            #endregion
        
            //Make the call
            HttpRequestMessage req = service.PrepareHttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri(con.ToString());
            HttpClient hc = new HttpClient();
            HttpResponseMessage resp = await hc.SendAsync(req);
            string content = await resp.Content.ReadAsStringAsync();
            
            //Error if not 200ok
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Earnings call request to Aletheia resulted with code " + resp.StatusCode.ToString() + ": " + content);
            }

            //Try to parse it
            EarningsCall ToRespondWith = null;
            try
            {
                ToRespondWith = JsonConvert.DeserializeObject<EarningsCall>(content);
            }
            catch (Exception ex)
            {
                throw new Exception("Fatal error while attempting to interpret payload from Aletheia: " + ex.Message);
            }

            //Return
            return ToRespondWith;
        }
    }
}