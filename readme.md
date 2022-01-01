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

### Accessing Earnings Call Transcripts
Once you know what specific earnings call (company, year, quarter) you would like to access, use Aletheia's [**Earnings Call**](https://aletheiaapi.com/docs/#earnings-call) service.
```
EarningsCall call = await service.GetEarningsCallAsync("MSFT", 2022, FiscalPeriod.Q1, 0, 10, true);
```
In the above line of code:
- "MSFT" = the company which you are requesting a transcript for
- 2022 = The fiscal year
- FiscalPeriod.Q1 = the quarter
- 0 = The starting remark count to request
- 10 = The ending remark count to request (this line of code is requesting remarks number 0 through 10)
- true = Specifying we would also like to be returned the total number of remarks available in this earnings call. This could be helpful to calculate how many further calls you need to request the remainder of the transcript.

The above line will return a response like this:

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
   "RemarksCount":129,
   "Remarks":[
      {
         "SequenceNumber":0,
         "Remark":"Greetings, and welcome to the Microsoft fiscal year 2022 first-quarter earnings conference call. At this time, all participants are in a listen-only mode. A question-and-answer session will follow our formal presentation. [Operator instructions] As a reminder, this conference is being recorded. It is now my pleasure to introduce your host, Brett Iversen, general manager and investor relations. Thank you.",
         "SpokenBy":{
            "Id":"bc18a561-7a1e-427e-be39-c2644eda4a11",
            "Name":"Operator",
            "Title":"Operator",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":1,
         "Remark":"You may begin.",
         "SpokenBy":{
            "Id":"bc18a561-7a1e-427e-be39-c2644eda4a11",
            "Name":"Operator",
            "Title":"Operator",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":2,
         "Remark":"Good afternoon, and thank you for joining us today. On the call with me are Satya Nadella, chairman and chief executive officer; Amy Hood, chief financial officer; Alice Jolla, chief accounting officer; and Keith Dolliver, deputy general counsel. On the Microsoft Investor Relations website, you can find our earnings press release and financial summary slide deck, which is intended to supplement our prepared remarks during today's call and provides the reconciliation of differences between GAAP and non-GAAP financial measures. Unless otherwise specified, we will refer to the non-GAAP metrics on the call.",
         "SpokenBy":{
            "Id":"4bb5cf22-e520-492a-83a9-d86e87a45e91",
            "Name":"Brett Iversen",
            "Title":"General Manager and Investor Relations",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":3,
         "Remark":"The non-GAAP financial measures provided should not be considered as a substitute for or superior to the measures of financial performance prepared in accordance with GAAP. They are included as additional clarifying items to aid investors in further understanding the company's first-quarter performance in addition to the impact these items and events have on the financial results. All growth comparisons we make on the call today relate to the corresponding period of last year unless otherwise noted. We'll also provide growth rates in constant currency when available as a framework for assessing how our underlying businesses performed, excluding the effect of foreign currency rate fluctuations. Where growth rates are the same and constant-currency, we refer to the growth rate only. We will post our prepared remarks to our website immediately following the call until the complete transcript is available.",
         "SpokenBy":{
            "Id":"4bb5cf22-e520-492a-83a9-d86e87a45e91",
            "Name":"Brett Iversen",
            "Title":"General Manager and Investor Relations",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":4,
         "Remark":"Today's call is being webcast live and recorded. If you ask a question, it will be included in our live transmission, in the transcript, and in any future use of the recording. You can replay the call and view transcripts on the Microsoft Investor Relations website. During this call, we will be making forward-looking statements, which are predictions, projections, or other statements about future events.",
         "SpokenBy":{
            "Id":"4bb5cf22-e520-492a-83a9-d86e87a45e91",
            "Name":"Brett Iversen",
            "Title":"General Manager and Investor Relations",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":5,
         "Remark":"These statements are based on current expectations and assumptions that are subject to risks and uncertainties. Actual results could materially differ because of factors discussed in today's earnings press release, in the comments made during this conference call, and in the Risk Factors section of our Form 10-K, Forms 10-Q, and other reports and filings with the Securities and Exchange Commission. We do not undertake any duty to update any forward-looking statements. And with that, I'll turn the call over to Satya.",
         "SpokenBy":{
            "Id":"4bb5cf22-e520-492a-83a9-d86e87a45e91",
            "Name":"Brett Iversen",
            "Title":"General Manager and Investor Relations",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":6,
         "Remark":"Thank you, Brett. We're off to a fast start in fiscal 2022 with Microsoft Cloud quarterly revenue surpassing $20 billion for the first time, up 36% year over year. The case for digital transformation has never been more urgent or more clear. Digital technology is a deflationary force in an inflationary economy.",
         "SpokenBy":{
            "Id":"b5aab7ce-7503-4e01-8b52-3feaa72f6d94",
            "Name":"Satya Nadella",
            "Title":"Chairman and Chief Executive Officer",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":7,
         "Remark":"Businesses small and large can improve productivity and the affordability of their products and services by building tech in density. The Microsoft Cloud delivers the end-to-end platforms and tools organizations need to navigate this time of transition and change. Now, I'll highlight examples of our innovation and momentum, starting with Azure. Every organization will need a distributed computing fabric across the cloud and the edge to rapidly build, manage, and deploy applications anywhere. We are building Azure as the world's computer, with more data center regions than any other provider, delivering fast access to cloud services while addressing critical data residency requirements. And we're partnering with mobile operators from AT&T and Verizon in the United States to Telefonica and BT in Europe, Telstra and Singtel in Asia Pacific, as they embrace new business models and bring ultra-low latency, compute power, and storage to the network and the enterprise edge.",
         "SpokenBy":{
            "Id":"b5aab7ce-7503-4e01-8b52-3feaa72f6d94",
            "Name":"Satya Nadella",
            "Title":"Chairman and Chief Executive Officer",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":8,
         "Remark":"Seventy-eight percent of the Fortune 500 use our hybrid offerings. And with Azure Arc, customers like Nokia, Royal Bank of Canada, and SKF can deploy and run applications at the edge or in multicloud environments. Organizations also prefer our cloud to power the mission-critical apps they rely on every day. GE Healthcare and Procter and Gamble migrated critical workloads to Azure this quarter. And leading companies in every industry including Bertelsmann, Kimberly Clark, The NBA, SoftBank, ThyssenKrupp all chose Azure for their SAP workloads. Now on to data. The leading indicator of digital transformation success in an organization is organization's ability to turn data into analytical and predictive bar.",
         "SpokenBy":{
            "Id":"b5aab7ce-7503-4e01-8b52-3feaa72f6d94",
            "Name":"Satya Nadella",
            "Title":"Chairman and Chief Executive Officer",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":9,
         "Remark":"Azure Synapse bring together -- brings together data integration, enterprise data warehousing, and big data analytics into a comprehensive solution. With Synapse Link for Dataverse, organizations can analyze data from business applications like Power Platform and Dynamics 365 with just a few clicks. With Synapse Link for Cosmos DB, they can run real-time no ETL analytics over their operational data. And with Power BI, anyone in the organization can access these insights. Thousands of active Power BI customers are using Synapse today.",
         "SpokenBy":{
            "Id":"b5aab7ce-7503-4e01-8b52-3feaa72f6d94",
            "Name":"Satya Nadella",
            "Title":"Chairman and Chief Executive Officer",
            "IsExternal":false
         }
      },
      {
         "SequenceNumber":10,
         "Remark":"More than ever, every business needs a holistic understanding of its data estate, and Azure Purview, now generally available, is helping organizations such as FedEx manage and govern on-premise multicloud and SaaS data. Now on to developers. As every company becomes a digital company, they will need standardized tools to modernize existing apps and build new ones. From GitHub to Visual Studio, we have the most used and loved developer tools to build any app on any platform. GitHub is now home to 73 million developers up 2 times since our acquisition three years ago.",
         "SpokenBy":{
            "Id":"b5aab7ce-7503-4e01-8b52-3feaa72f6d94",
            "Name":"Satya Nadella",
            "Title":"Chairman and Chief Executive Officer",
            "IsExternal":false
         }
      }
   ]
}
```