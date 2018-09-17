using System;
using System.Collections.Generic;

namespace POC.Core.Logging
{
    public class LogDetail
    {
        public LogDetail()
        {
            Timestamp = DateTime.Now;
        }

        //BASIC
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        //WHERE
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }

        //WHO
        public string UserId { get; set; }
        public string UserName { get; set; }

        //EVERYTING ELSE
        public long? ElapsedMilliseconds { get; set; } // only for performance entries
        public Exception Exception { get; set; } // the exception for error logging
        public string CorrelationId { get; set; } // exceptio shielding from server to client
        public Dictionary<string, object> AdditionalInfo { get; set; } // catch-all for anything else. 
    }
}
