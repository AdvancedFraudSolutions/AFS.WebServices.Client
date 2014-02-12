namespace AFS.WebServices.Client.TrueChecks
{
    /// <summary>
    /// The response body for getting a customer's TrueChecks settings.
    /// </summary>
    public class TrueChecksClientSettingsResponse
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
    }
}