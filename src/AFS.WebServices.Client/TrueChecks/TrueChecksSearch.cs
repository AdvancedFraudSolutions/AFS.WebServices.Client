using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Xml.Linq;

namespace AFS.WebServices.Client.TrueChecks
{
    /// <summary>
    /// The request body of a TrueChecks search.
    /// </summary>
    public class TrueChecksSearch : ISerializeToRequestStream
    {
        public TrueChecksSearch()
        {
            IsBusinessOrPersonalCheck = true;
            DoTrueChecksSearch = true;
            DoDepositChekSearch = true;
        }

        /// <summary>
        /// Whether the check being presented is a business/personal check or not.
        /// </summary>
        public bool IsBusinessOrPersonalCheck { get; set; }

        /// <summary>
        /// The routing number of the check being presented.
        /// </summary>
        public string RoutingNumber { get; set; }

        /// <summary>
        /// The account number of the check being presented.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The check number of the check being presented.
        /// </summary>
        public string CheckNumber { get; set; }

        /// <summary>
        /// The amount of the check being presented.
        /// </summary>
        public decimal CheckAmount { get; set; }

        /// <summary>
        /// The maker of the check being presented.
        /// </summary>
        public string Maker { get; set; }

        /// <summary>
        /// The first name of the depositor. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the depositor. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The SSN of the depositor. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The financial institution ID of the depositor. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string FIID { get; set; }

        /// <summary>
        /// The drivers license state of the depositor. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string DriversLicenseState { get; set; }

        /// <summary>
        /// The drivers license number of the depositor. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string DriversLicenseNumber { get; set; }

        /// <summary>
        /// The teller's ID. This is an arbitrary value that identifies the teller for reporting purposes.
        /// </summary>
        public string TellerId { get; set; }

        /// <summary>
        /// The teller's branch ID. This is an arbitrary value that identifies the teller for reporting purposes.
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// If this query is a refinement of a previous query, the previous query's ID should be supplied here.
        /// </summary>
        public int? RefinedQueryId { get; set; }

        /// <summary>
        /// Whether the TrueChecks service should be used for this request. The default is true.
        /// </summary>
        public bool DoTrueChecksSearch { get; set; }

        /// <summary>
        /// Whether the DepositChek service should be used for this request. The default is true.
        /// </summary>
        public bool DoDepositChekSearch { get; set; }

        public void SerializeToRequestStream(Stream stream, string contentType)
        {
            this.EnsureContentType(contentType, "application/x-www-form-urlencoded");


            // reference: https://api.advancedfraudsolutions.com/Api/POST-TrueChecks-Search

            using (var writer = new StreamWriter(stream))
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["AccountNumber"] = AccountNumber;
                parameters["BranchId"] = BranchId;
                parameters["CheckAmount"] = CheckAmount.ToString(CultureInfo.InvariantCulture);
                parameters["CheckNumber"] = CheckNumber;
                parameters["DoDepositChekSearch"] = DoDepositChekSearch.ToString();
                parameters["DoTrueChecksSearch"] = DoTrueChecksSearch.ToString();
                parameters["DriversLicenseNumber"] = DriversLicenseNumber;
                parameters["DriversLicenseState"] = DriversLicenseState;
                parameters["FIID"] = FIID;
                parameters["FirstName"] = FirstName;
                parameters["IsBusinessOrPersonalCheck"] = IsBusinessOrPersonalCheck.ToString();
                parameters["LastName"] = LastName;
                parameters["Maker"] = Maker;
                parameters["RefinedQueryId"] = RefinedQueryId.ToString();
                parameters["RoutingNumber"] = RoutingNumber;
                parameters["SSN"] = SSN;
                parameters["TellerId"] = TellerId;
                writer.Write(parameters.ToString());
            }
        }
    }
}