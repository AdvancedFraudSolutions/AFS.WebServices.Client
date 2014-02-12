namespace AFS.WebServices.Client.TrueChecks
{
    /// <summary>
    /// The request body used when recording a teller's action for a check.
    /// </summary>
    public class TrueChecksAction
    {
        /// <summary>
        /// The ID of the TrueChecks query for which this action references.
        /// </summary>
        public int QueryId { get; set; }

        /// <summary>
        /// The action that the teller took on the check.
        /// </summary>
        public QueryAction Action { get; set; }
    }
}