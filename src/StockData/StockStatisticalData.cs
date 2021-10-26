using System;

namespace Aletheia.Service.StockData
{
    public class StockStatisticalData
    {
        //Fiscal Year
        public DateTime? FiscalYearEnds {get; set;}
        public DateTime? MostRecentQuarter {get; set;}

        //Profitability
        public float? ProfitMargin {get; set;}
        public float? OperatingMargin {get; set;}

        //Management Affectiveness
        public float? ReturnOnAssets {get; set;}
        public float? ReturnOnEquity {get; set;}

        //Income statement
        public long? Revenue {get; set;}
        public float? RevenuePerShare {get; set;}
        public float? QuarterlyRevenueGrowth {get; set;}
        public long? GrossProfit {get; set;}
        public long? EBITDA {get; set;}
        public long? NetIncomeAvailableToCommon {get; set;}
        public float? DilutedEps {get; set;}
        public float? QuarterlyEarningsGrowth {get; set;}

        //Balance Sheet
        public long? TotalCash {get; set;}
        public float? TotalCashPerShare {get; set;}
        public long? TotalDebt {get; set;}
        public float? TotalDebtEquityRatio {get; set;}
        public float? CurrentRatio {get; set;}
        public float? BookValuePerShare {get; set;}

        //Cash Flow Statement
        public long? OperatingCashFlow {get; set;}
        public long? LeveredFreeCashFlow {get; set;}

        //Stock price information
        public float? Beta {get; set;}
        public float? YearChange {get; set;}
        public float? SP500YearChange {get; set;}
        public float? YearHigh {get; set;}
        public float? YearLow {get; set;}
        public float? MovingAverage50Day {get; set;}
        public float? MovingAverage200Day {get; set;}

        //Share statistics
        public long? AverageVolume3Month {get; set;}
        public long? AverageVolume10Day {get; set;}
        public long? SharesOutstanding {get; set;}
        public long? Float {get; set;}
        public float? PercentHeldByInsiders {get; set;}
        public float? PercentHeldByInstitutions {get; set;}
        public long? SharesShort {get; set;}
        public float? ShortRatio {get; set;}
        public float? ShortPercentOfFloat {get; set;}
        public float? ShortPercentOfSharesOutstanding {get; set;}

        //Dividends & Splits
        public float? ForwardAnnualDividend {get; set;}
        public float? ForwardAnnualDividendYield {get; set;}
        public float? TrailingAnnualDividend {get; set;}
        public float? TrailingAnnualDividendYield {get; set;}
        public float? FiveYearAverageDividendYield {get; set;}
        public float DividendPayoutRatio {get; set;}
        public DateTime? DividendDate {get; set;}
        public DateTime? ExDividendDate {get; set;}
        public string LastSplitFactor {get; set;}
        public DateTime? LastSplitDate {get; set;}
    }
}