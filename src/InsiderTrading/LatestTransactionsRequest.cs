using System;

namespace Aletheia.Service.InsiderTrading
{
    public class LatestTransactionsRequest
    {
        public string Issuer {get; set;}
        public string Owner {get; set;}
        public int? Top {get; set;}
        public DateTime? Before {get; set;}
        public SecurityType? SecurityType {get; set;}
        public TransactionType? TransactionType {get; set;}
        public bool? Cascade {get; set;}
    }
}