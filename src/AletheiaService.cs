using System;
using System.Net.Http;
using System.Net;

namespace Aletheia.Service
{
    public class AletheiaService
    {
        private Guid _ApiKey;

        public AletheiaService(Guid api_key)
        {
            _ApiKey = api_key;
        }

        public AletheiaService(string api_key)
        {
            Guid key;
            try
            {
                key = Guid.Parse(api_key);
            }
            catch
            {
                throw new Exception("Value '" + api_key + "' is not a valid Aletheia API key.");
            }
            _ApiKey = key;
        }
    
        public Guid ApiKey
        {
            get
            {
                return _ApiKey;
            }
        }
    
    }
}
