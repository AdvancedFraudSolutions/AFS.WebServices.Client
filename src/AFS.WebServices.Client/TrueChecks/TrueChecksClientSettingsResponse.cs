using System;
using System.IO;
using System.Xml.Linq;

namespace AFS.WebServices.Client.TrueChecks
{
    /// <summary>
    /// The response body for getting a customer's TrueChecks settings.
    /// </summary>
    public class TrueChecksClientSettingsResponse : IDeserializeFromResponseStream
    {
        /// <summary>
        /// Whether depositor searching is enabled for the customer.
        /// </summary>
        public bool DepositorSearchEnabled { get; set; }

        /// <summary>
        /// Controls searching by depositor's SSN.
        /// </summary>
        public FieldMode SSNMode { get; set; }

        /// <summary>
        /// Controls searching by depositor's financial institution ID.
        /// </summary>
        public FieldMode FIIDMode { get; set; }

        /// <summary>
        /// Controls searching by depositor's drivers license.
        /// </summary>
        public FieldMode DriversLicenseMode { get; set; }

        public void DeserializeFromResponseStream(Stream stream, string contentType)
        {
            this.EnsureContentTypeStartsWith(contentType, "application/xml");

            // reference: https://api.advancedfraudsolutions.com/Api/GET-TrueChecks-ClientSettings
            var tcns = XNamespace.Get("http://schemas.datacontract.org/2004/07/AFS.WebAPI.Models.TrueChecks");

            XElement xml;
            using (var reader = new StreamReader(stream))
                xml = XElement.Load(reader);

            DepositorSearchEnabled = xml.Element(tcns + "DepositorSearchEnabled").Value.Parse(bool.Parse);
            DriversLicenseMode = xml.Element(tcns + "DriversLicenseMode").Value.Parse(ParseFieldMode);
            FIIDMode = xml.Element(tcns + "FIIDMode").Value.Parse(ParseFieldMode);
            SSNMode = xml.Element(tcns + "SSNMode").Value.Parse(ParseFieldMode);
        }

        private static FieldMode ParseFieldMode(string s)
        {
            return (FieldMode)Enum.Parse(typeof(FieldMode), s);
        }
    }
}