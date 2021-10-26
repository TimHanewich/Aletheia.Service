using System;

namespace Aletheia.Service.StockData
{
    public class StockSummaryData
    {
        public string Name { get; set; }
        public string StockSymbol { get; set; }
        public float Price { get; set; }
        public float DollarChange { get; set; }
        public float PercentChange { get; set; }
        public float PreviousClose { get; set; }
        public float Open { get; set; }
        public float BidPrice { get; set; }
        public int BidQuantity { get; set; }
        public float AskPrice { get; set; }
        public int AskQuantity { get; set; }
        public float DayRangeLow { get; set; }
        public float DayRangeHigh { get; set; }
        public float YearRangeLow { get; set; }
        public float YearRangeHigh { get; set; }
        public long Volume { get; set; }
        public long AverageVolume { get; set; }
        public double MarketCap { get; set; }
        public float? Beta { get; set; }
        public float? PriceEarningsRatio { get; set; }
        public float EarningsPerShare { get; set; }
        public DateTime? EarningsDate { get; set; }
        public float? ForwardDividend { get; set; }
        public float? ForwardDividendYield { get; set; }
        public DateTime? ExDividendDate { get; set; }
        public float YearTargetEstimate { get; set; }
    }
}