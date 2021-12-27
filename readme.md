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