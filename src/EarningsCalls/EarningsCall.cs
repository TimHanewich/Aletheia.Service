using System;
using Aletheia.Service.InsiderTrading;

namespace Aletheia.Service.EarningsCalls
{
    public class EarningsCall
    {
        public Guid Id {get; set;}
        public Company Company {get; set;}
        public FiscalPeriod Period {get; set;}
        public int Year {get; set;}
        public DateTime HeldAt {get; set;}
        public SpokenRemark[] Remarks {get; set;}
    }
}