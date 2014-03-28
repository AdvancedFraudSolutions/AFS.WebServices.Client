using System.Globalization;
using System.IO;
using System.Web;

namespace AFS.WebServices.Client.TrueChecks
{
    /// <summary>
    /// The request body used when recording a teller's action for a check.
    /// </summary>
    public class TrueChecksAction : ISerializeToRequestStream
    {
        /// <summary>
        /// The ID of the TrueChecks query for which this action references.
        /// </summary>
        public int QueryId { get; set; }

        /// <summary>
        /// The action that the teller took on the check.
        /// </summary>
        public QueryAction Action { get; set; }

        public void SerializeToRequestStream(Stream stream, string contentType)
        {
            this.EnsureContentType(contentType, "application/x-www-form-urlencoded");

            using (var writer = new StreamWriter(stream))
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["QueryId"] = QueryId.ToString(CultureInfo.InvariantCulture);
                parameters["Action"] = Action.ToString();
                writer.Write(parameters.ToString());
            }
        }
    }
}