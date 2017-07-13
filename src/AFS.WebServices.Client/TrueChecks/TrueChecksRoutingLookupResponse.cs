using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFS.WebServices.Client.TrueChecks
{
    public class TrueChecksRoutingLookupResponse
    {
        public string RoutingNumber { get; }
        public bool IsValidUSRoutingNumber { get; }
        public bool IsCanadianRoutingNumber { get; }
        public RoutingInfo RoutingInfo { get; }
    }
}