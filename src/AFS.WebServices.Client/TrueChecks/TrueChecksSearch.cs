﻿namespace AFS.WebServices.Client.TrueChecks
{
    /// <summary>
    /// The request body of a TrueChecks search.
    /// </summary>
    public class TrueChecksSearch
    {
        /// <summary>
        /// Whether the check being presented is a business/personal check or not. The default is true.
        /// </summary>
        public bool IsBusinessOrPersonalCheck { get; set; } = true;

        /// <summary>
        /// The routing number of the check being presented. Required.
        /// </summary>
        public string RoutingNumber { get; set; }

        /// <summary>
        /// The account number of the check being presented. Required.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The check number of the check being presented. Required.
        /// </summary>
        public string CheckNumber { get; set; }

        /// <summary>
        /// The amount of the check being presented. Required.
        /// </summary>
        public decimal CheckAmount { get; set; }

        /// <summary>
        /// The maker of the check being presented.
        /// </summary>
        public string Maker { get; set; }

        /// <summary>
        /// The first name of the depositor. A value may be required, optional, or disallowed depending. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the depositor. A value may be required, optional, or disallowed depending. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The SSN of the depositor. A value may be required, optional, or disallowed depending. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The financial institution ID of the depositor. A value may be required, optional, or disallowed depending. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string FIID { get; set; }

        /// <summary>
        /// The drivers license state of the depositor. A value may be required, optional, or disallowed depending. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string DriversLicenseState { get; set; }

        /// <summary>
        /// The drivers license number of the depositor. A value may be required, optional, or disallowed depending. Check the customer's TrueChecks settings before supplying this value in a query.
        /// </summary>
        public string DriversLicenseNumber { get; set; }

        /// <summary>
        /// The teller's ID. This is an arbitrary value that identifies the teller for reporting purposes. Required.
        /// </summary>
        public string TellerId { get; set; }

        /// <summary>
        /// The teller's branch ID. This is an arbitrary value that identifies the teller's branch for reporting purposes. Required.
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// If this query is a refinement of a previous query, the previous query's ID should be supplied here.
        /// </summary>
        public int? RefinedQueryId { get; set; }

        /// <summary>
        /// Whether the TrueChecks service should be used for this request. The default is true.
        /// </summary>
        public bool DoTrueChecksSearch { get; set; } = true;

        /// <summary>
        /// Whether the DepositChek service should be used for this request. The default is true.
        /// </summary>
        public bool DoDepositChekSearch { get; set; } = true;
    }
}