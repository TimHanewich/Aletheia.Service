using System;

namespace Aletheia.Service.EarningsCalls
{
    public class SpokenRemark
    {
        public int SequenceNumber {get; set;}
        public string Remark {get; set;}
        public CallParticipant SpokenBy {get; set;}
    }
}