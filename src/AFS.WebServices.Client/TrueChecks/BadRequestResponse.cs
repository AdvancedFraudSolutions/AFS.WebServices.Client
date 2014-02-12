using System.Collections.Generic;

namespace AFS.WebServices.Client.TrueChecks
{

    /// <summary>
    /// Information about the bad request.
    /// </summary>
    public class BadRequestResponse
    {
        /// <summary>
        /// A message describing why a bad request status was returned.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A collection of error messages.
        /// </summary>
        public Dictionary<string, string[]> ModelState { get; set; }
    }
}
