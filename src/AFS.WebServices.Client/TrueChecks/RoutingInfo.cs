namespace AFS.WebServices.Client.TrueChecks
{
    public class RoutingInfo
    {
        public string CustomerName { get; internal set; }
        public string Address { get; internal set; }
        public string City { get; internal set; }
        public string State { get; internal set; }
        public string ZipCode { get; internal set; }
        public string ZipCodeExtension { get; internal set; }
        public string TelephoneAreaCode { get; internal set; }
        public string TelephonePrefix { get; internal set; }
        public string TelephoneSuffix { get; internal set; }
    }
}