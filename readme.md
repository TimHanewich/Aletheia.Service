# Aletheia.Service

[Aletheia](https://aletheiaapi.com) is a financial REST API that provides all types of financial data from stock quotes, insider trading history, earnings call transcripts, financial statements, and more.  

Use this class library for transacting with the Aletheia API in your .NET-based projects.

## Installation
To use this library, install the [**Aletheia.Service**](https://www.nuget.org/packages/Aletheia.Service/) nuget package.

To install via the command line:
```
dotnet add package Aletheia.Service
```

## Authentication
To use the Aletheia API service, you will need to register as a developer to receive an **Aletheia API Key**. Head to [Aletheia's login page](https://aletheiaapi.com/login/) to register for free.

Your Aletheia API key will look something like `24DD37C742D0412C90A62771C582FA6C`.

## Service Initialization
All transactions with the Aletheia API service are performed through the `AletheiaService` class:

```
using Aletheia;

AletheiaService service = new AletheiaService("<your api key>");
```

## Stock Data
After importing the `Aletheia.Service.StockData` namespace, use the `GetStockDataAsync` method to download a quote and additional data for any publicly traded US equity:

```
using Aletheia.Service.StockData;

StockData quote = await service.GetStockDataAsync("PG", true, true);
Console.WriteLine("Name: " + quote.SummaryData.Name);
Console.WriteLine("Last price: " + quote.SummaryData.Price.ToString("#,##0.00"));
Console.WriteLine("Dividend payout ratio: " + quote.StatisticalData.DividendPayoutRatio.ToString("#0.0%"));
            
```
The above code snippet demonstrated downloading summary and statistical data from Aletheia. A few data points are written to the console, but many more data points are available.

### Downloading Data for Multiple Stocks
You can also simultaneously download data for many stocks at once. For example:
```
StockData[] allData = await service.GetMultipleStockDataAsync(new string[] {"PG", "MSFT", "INTC", "MMM", "KO"}, true, true);
```
However, if a single quote fails from the list, the entire method will fail. For example, if a quote is not available for a stock or a provided symbol is invalid. To avoid this, use the `TryGetMultipleStockDataAsync` method:
```
StockData[] allData = await service.TryGetMultipleStockDataAsync(new string[] {"PG", "MSFT", "INTC", "MMM", "KO"}, true, true);
```
The `TryGetMultipleStockDataAsync` method will simply ignore the symbols that failed and will only return the successfully downloaded data packages.

## Insider Trading
Aletheia also provides data on insider trades of public equity and options by executives, board members, officials, etc. 

The primary service that exposes insider trading data is the [**Latest Transactions**](https://aletheiaapi.com/docs/#latest-transactions) endpoint. To consume the **Latest Transactions** service:
```
AletheiaService service = new AletheiaService("<your key here>");
LatestTransactionsRequest req = new LatestTransactionsRequest();
req.Issuer = "MSFT"; //Stock symbol of the company I am interested in seeing trades of.
req.Owner = "1513142"; //CIK # of Satya Nadella, CEO of Microsoft. So show insider trades of MSFT stock by Satya Nadella.
req.SecurityType = SecurityType.NonDerivative; //Only show non-derivatve assets (stock, not options)
req.TransactionType = TransactionType.OpenMarketOrPrivatePurchase; //Return transactions where Nadella purchased MSFT stock only
req.Before = new DateTime(2019, 7, 1); //Include the most recent results before July 1, 2019.
req.Top = 10; //Only return a maximum of 10 results
req.Cascade = true; //Include data about the SEC filing this trade originated from and the owner/issuer involved.
SecurityTransactionHolding[] sths = await service.LatestTransactions(req);
```
As seen above, there are many parameters you can specify to filter the insider trading results. You can omit any of these to broaden the search results.

An example of one of records returned:
```
{
   "Id":"f5f63263-8d35-4085-a1cf-c4c1243a2b6d",
   "FromFiling":"65bb76c3-fd57-4d34-9425-89369a4d752d",
   "_FromFiling":{
      "Id":"65bb76c3-fd57-4d34-9425-89369a4d752d",
      "FilingUrl":"https://www.sec.gov/Archives/edgar/data/1186249/000106299321011817/0001062993-21-011817-index.htm",
      "AccessionP1":1062993,
      "AccessionP2":21,
      "AccessionP3":11817,
      "FilingType":0,
      "ReportedOn":"2021-11-29T00:00:00",
      "Issuer":789019,
      "_Issuer":{
         "Cik":789019,
         "Name":"MICROSOFT CORP",
         "TradingSymbol":"MSFT"
      },
      "Owner":1186249,
      "_Owner":{
         "Cik":1186249,
         "Name":"WARRIOR PADMASREE",
         "TradingSymbol":null
      }
   },
   "EntryType":0,
   "QuantityOwnedFollowingTransaction":12194.0,
   "DirectIndirect":0,
   "SecurityTitle":"Common Stock",
   "SecurityType":0,
   "AcquiredDisposed":0,
   "Quantity":148.0,
   "PricePerSecurity":null,
   "TransactionDate":"2021-11-29T00:00:00",
   "TransactionCode":3,
   "ConversionOrExercisePrice":null,
   "ExercisableDate":null,
   "ExpirationDate":null,
   "UnderlyingSecurityTitle":null,
   "UnderlyingSecurityQuantity":null
}
```

### Search Entities
As seen in the **Latest Transactions** documentation above, you can specify the CIK (Central Index Key, a unique SEC-assigned ID) of the issuer (company) or owner (insider) to filter the results to only this issuer or owner.
To find the CIK of the issuer or owner ("entity") that you are interested in, use the [**Search Entities**](https://api.aletheiaapi.com/SearchEntities) service:

```
AletheiaService service = new AletheiaService("<your key here>");
Entity[] entities = await service.SearchEntitiesAsync("cook", 10);
```
An example of a record from the search results:
```
{
   "Cik":1214156,
   "Name":"COOK TIMOTHY D",
   "TradingSymbol":null
}
```
Records that relate to an *issuer* (company), will have a Trading Symbol:
```
{
   "Cik":320193,
   "Name":"APPLE COMPUTER INC",
   "TradingSymbol":"AAPL"
}
```

## Earnings Calls
Aletheia can provide transcripts for earnings calls of publicly traded companies. To search amongst the available transcripts:

### Earnings Call Transcript Searching
The above code snippet searches Aletheia for any transcript from a **Microsoft** ($MSFT) earnings call:
```
EarningsCall[] calls = await service.SearchEarningsCallsAsync("MSFT", null, null, null);
```
You can also further refine your search to a specific year or quarter. For example, searching for Microsoft Q1 2022 earnings call:
```
EarningsCall[] calls = await service.SearchEarningsCallsAsync("MSFT", 2022, FiscalPeriod.Q1, null);
```
The **Search Earnings Calls** service will return an array of records that look like this:
```
{
   "Id":"adca5f26-c1cb-42e1-b594-8829f2de39d4",
   "Company":{
      "Id":"5372f27b-929c-45f6-8ae5-7404a59d307f",
      "Name":"Microsoft",
      "TradingSymbol":"MSFT"
   },
   "Period":0,
   "Year":2022,
   "HeldAt":"2021-10-26T00:00:00",
}
```
