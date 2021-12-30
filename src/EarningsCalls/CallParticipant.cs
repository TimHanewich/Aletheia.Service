using System;

namespace Aletheia.Service.EarningsCalls
{
    public class CallParticipant
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public string Title {get; set;}
        public bool IsExternal {get; set;}
    }
}