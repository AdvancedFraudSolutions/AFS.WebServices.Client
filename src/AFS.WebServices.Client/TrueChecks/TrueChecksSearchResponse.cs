using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace AFS.WebServices.Client.TrueChecks
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The response body of a TrueChecks search.
    /// </summary>
    public class TrueChecksSearchResponse : IDeserializeFromResponseStream
    {
        /// <summary>
        /// Whether the customer is licensed to use the TrueChecks service or not.
        /// </summary>
        public bool HasTrueChecksLicense { get; set; }

        /// <summary>
        /// Whether the customer is licensed to use the DepositChek service or not.
        /// </summary>
        public bool HasDepositChekLicense { get; set; }

        /// <summary>
        /// The ID of the query. This is used when recording a Teller's action.
        /// </summary>
        public int QueryId { get; set; }

        /// <summary>
        /// The overall recommendation given by Advanced Fraud Solutions, considering all results.
        /// </summary>
        public string OverallRecommendedAction { get; set; }

        /// <summary>
        /// A collection of the TrueChecks check alert results.
        /// </summary>
        public CheckAlertResult[] CheckAlertResults { get; set; }

        /// <summary>
        /// A collection of the TrueChecks depositor results.
        /// </summary>
        public PersonResult[] PersonResults { get; set; }

        /// <summary>
        /// Whether the EWS DepositChek service is operational or not. False if not operational, true otherwise.
        /// </summary>
        public bool IsEWSDepositChekServiceDown { get; set; }

        /// <summary>
        /// The account status results given by EWS DepositChek.
        /// </summary>
        public DepositChekResult DepositChekResults { get; set; }

        /// <summary>
        /// A TrueChecks check alert result.
        /// </summary>
        public class CheckAlertResult
        {
            /// <summary>
            /// The check's routing number.
            /// </summary>
            public string RoutingNumber { get; set; }

            /// <summary>
            /// The check's account number.
            /// </summary>
            public string AccountNumber { get; set; }

            /// <summary>
            /// The check's serial number.
            /// </summary>
            public string CheckNumber { get; set; }

            /// <summary>
            /// The check's amount.
            /// </summary>
            public decimal Amount { get; set; }

            /// <summary>
            /// The check's maker's name.
            /// </summary>
            public string MakerName { get; set; }

            /// <summary>
            /// The check's maker's address.
            /// </summary>
            public string MakerAddress { get; set; }

            /// <summary>
            /// The relative path used to retrieve the check's front side image.
            /// This is used to retrieve an image from the TrueChecks web service.
            /// </summary>
            public string FrontImage { get; set; }

            /// <summary>
            /// The type of check.
            /// </summary>
            public string CheckType { get; set; }

            /// <summary>
            /// The type of fraud that was recorded for the check.
            /// </summary>
            public string FraudType { get; set; }

            /// <summary>
            /// An id number for the fraud type.
            /// </summary>
            public int FraudTypeId { get; set; }

            /// <summary>
            /// The recommendation given by the Advanced Fraud Solutions for this check alert.
            /// </summary>
            public string RecommendedAction { get; set; }

            /// <summary>
            /// The relative weight of the Recommendation.
            /// This is used to for ordering multiple results in the 
            /// user interface and for determining the overrall recommendation.
            /// </summary>
            public int RecommendedActionWeight { get; set; }

            /// <summary>
            /// Notes about the alert.
            /// </summary>
            public string Notes { get; set; }

            /// <summary>
            /// When the alert was created.
            /// </summary>
            public DateTimeOffset CreatedDate { get; set; }

            /// <summary>
            /// When the alert was last updated.
            /// </summary>
            public DateTimeOffset UpdatedDate { get; set; }
        }

        /// <summary>
        /// The results given by the EWS DepositChek service.
        /// See the EWS DepositChek documentation for more information about this data.
        /// </summary>
        public class DepositChekResult
        {
            public string ErrorTitle { get; set; }
            public string ErrorDescription { get; set; }

            /// <summary>
            /// Recommended action based on the ews response primary status code.
            /// </summary>
            public string RecommendedAction { get; set; }
            public string PrimaryStatusCode { get; set; }
            public string PrimaryStatusTitle { get; set; }
            public string PrimaryStatusShortTitle { get; set; }
            public string PrimaryStatusDescription { get; set; }
            public string PrimaryStatusHitType { get; set; }
            public string SecondaryStatusCode { get; set; }
            public string SecondaryStatusTitle { get; set; }
            public string SecondaryStatusDescription { get; set; }
            public string SecondaryStatusHitType { get; set; }
            public string AdditionalStatusCode { get; set; }
            public string AdditionalStatusTitle { get; set; }
            public string AdditionalStatusDescription { get; set; }
            public string AdditionalStatusHitType { get; set; }
            public string LastReturnReasonCode { get; set; }
            public string LastReturnReasonTitle { get; set; }
            public string LastReturnReasonDescription { get; set; }
            public string LastReturnReasonHitType { get; set; }
            public string PreviousStatusCode { get; set; }
            public string PreviousStatusTitle { get; set; }
            public string PreviousStatusDescription { get; set; }
            public string PreviousStatusHitType { get; set; }
        }

        /// <summary>
        /// Depositor details supplied by the financial institution.
        /// </summary>
        public class PersonResult
        {
            public int PersonId { get; set; }

            /// <summary>
            /// The depositor's first name.
            /// </summary>
            public string FirstName { get; set; }

            /// <summary>
            /// The depositor's last name.
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            /// The depositor's SSN.
            /// </summary>
            public string SSN { get; set; }

            /// <summary>
            /// The depositor's financial institution ID.
            /// </summary>
            public string FIID { get; set; }

            /// <summary>
            /// The depositor's drivers license state.
            /// </summary>
            public string DriversLicenseState { get; set; }

            /// <summary>
            /// The depositor's drivers license number.
            /// </summary>
            public string DriversLicense { get; set; }

            /// <summary>
            /// The relative path used to retrieve the depositor's image.
            /// This is used to retrieve an image from the TrueChecks web service.
            /// </summary>
            public string Image { get; set; }

            /// <summary>
            /// When the details of the alert were last updated.
            /// </summary>
            public DateTimeOffset PersonUpdated { get; set; }

            /// <summary>
            /// Whether the current search matched this alert on just name or on other more identifying information supplied.
            /// This should be used for ordering multiple results in the user interface.
            /// </summary>
            public bool NameMatchOnly { get; set; }

            /// <summary>
            /// A collection the types of activity perpetrated by the depositor and the number of occurrences on record.
            /// </summary>
            public Dictionary<string, int> ActivitySummary { get; set; }
        }

        public void DeserializeFromResponseStream(Stream stream, string contentType)
        {
            this.EnsureContentTypeStartsWith(contentType, "application/xml");

            // reference: https://api.advancedfraudsolutions.com/Api/POST-TrueChecks-Search

            var ns = XNamespace.Get("http://schemas.datacontract.org/2004/07/AFS.WebAPI.Models.TrueChecks");
            XElement xml;
            using (var reader = new StreamReader(stream))
                xml = XElement.Load(reader);

            // ReSharper disable PossibleNullReferenceException

            QueryId = xml.Element(ns + "QueryId").Value.Parse(Convert.ToInt32);
            HasDepositChekLicense = xml.Element(ns + "HasDepositChekLicense").Value.Parse(Convert.ToBoolean);
            HasTrueChecksLicense = xml.Element(ns + "HasTrueChecksLicense").Value.Parse(Convert.ToBoolean);
            IsEWSDepositChekServiceDown =
                xml.Element(ns + "IsEWSDepositChekServiceDown").Value.Parse(Convert.ToBoolean);
            OverallRecommendedAction = xml.Element(ns + "OverallRecommendedAction").Value;


            CheckAlertResults = xml.Element(ns + "CheckAlertResults")
                .Elements(ns + "TrueChecksSearchResponse.CheckAlertResult")
                .Select(el => new CheckAlertResult
                {
                    AccountNumber = el.Element(ns + "AccountNumber").Value,
                    Amount = el.Element(ns + "Amount").Value.Parse(Convert.ToDecimal),
                    CheckNumber = el.Element(ns + "CheckNumber").Value,
                    CheckType = el.Element(ns + "CheckType").Value,
                    CreatedDate = ResponseXmlParser.ParseDateTimeOffset(el.Element(ns + "CreatedDate")),
                    FraudType = el.Element(ns + "FraudType").Value,
                    FraudTypeId = el.Element(ns + "FraudTypeId").Value.Parse(Convert.ToInt32),
                    FrontImage = el.Element(ns + "FrontImage").Value,
                    MakerAddress = el.Element(ns + "MakerAddress").Value,
                    MakerName = el.Element(ns + "MakerName").Value,
                    Notes = el.Element(ns + "Notes").Value,
                    RecommendedAction = el.Element(ns + "RecommendedAction").Value,
                    RecommendedActionWeight =
                        el.Element(ns + "RecommendedActionWeight").Value.Parse(Convert.ToInt32),
                    RoutingNumber = el.Element(ns + "RoutingNumber").Value,
                    UpdatedDate = ResponseXmlParser.ParseDateTimeOffset(el.Element(ns + "UpdatedDate")),
                }).ToArray();


            PersonResults = xml.Element(ns + "PersonResults")
                .Elements(ns + "TrueChecksSearchResponse.PersonResult")
                .Select(el => new PersonResult
                {
                    ActivitySummary = ResponseXmlParser.ParseManyKeyValueOfStringInts(el.Element(ns + "ActivitySummary ")).ToDictionary(x => x.Key, x => x.Value),
                    DriversLicense = el.Element(ns + "DriversLicense ").Value,
                    DriversLicenseState = el.Element(ns + "DriversLicenseState ").Value,
                    FIID = el.Element(ns + "FIID ").Value,
                    FirstName = el.Element(ns + "FirstName ").Value,
                    Image = el.Element(ns + "Image ").Value,
                    LastName = el.Element(ns + "LastName ").Value,
                    NameMatchOnly = el.Element(ns + "NameMatchOnly").Value.Parse(Convert.ToBoolean),
                    PersonId = el.Element(ns + "PersonId").Value.Parse(Convert.ToInt32),
                    PersonUpdated = ResponseXmlParser.ParseDateTimeOffset(el.Element(ns + "PersonUpdated")),
                    SSN = el.Element(ns + "SSN").Value,

                }).ToArray();

            {
                var el = xml.Element(ns + "DepositChekResults");
                if (!el.IsEmpty)
                    DepositChekResults = new DepositChekResult
                    {
                        AdditionalStatusCode = el.Element(ns + "AdditionalStatusCode").Value,
                        AdditionalStatusDescription = el.Element(ns + "AdditionalStatusDescription").Value,
                        AdditionalStatusHitType = el.Element(ns + "AdditionalStatusHitType").Value,
                        AdditionalStatusTitle = el.Element(ns + "AdditionalStatusTitle").Value,
                        ErrorDescription = el.Element(ns + "ErrorDescription").Value,
                        ErrorTitle = el.Element(ns + "ErrorTitle").Value,
                        LastReturnReasonCode = el.Element(ns + "LastReturnReasonCode").Value,
                        LastReturnReasonDescription = el.Element(ns + "LastReturnReasonDescription").Value,
                        LastReturnReasonHitType = el.Element(ns + "LastReturnReasonHitType").Value,
                        LastReturnReasonTitle = el.Element(ns + "LastReturnReasonTitle").Value,
                        PreviousStatusCode = el.Element(ns + "PreviousStatusCode").Value,
                        PreviousStatusDescription = el.Element(ns + "PreviousStatusDescription").Value,
                        PreviousStatusHitType = el.Element(ns + "PreviousStatusHitType").Value,
                        PreviousStatusTitle = el.Element(ns + "PreviousStatusTitle").Value,
                        PrimaryStatusCode = el.Element(ns + "PrimaryStatusCode").Value,
                        PrimaryStatusDescription = el.Element(ns + "PrimaryStatusDescription").Value,
                        PrimaryStatusHitType = el.Element(ns + "PrimaryStatusHitType").Value,
                        PrimaryStatusShortTitle = el.Element(ns + "PrimaryStatusShortTitle").Value,
                        PrimaryStatusTitle = el.Element(ns + "PrimaryStatusTitle").Value,
                        RecommendedAction = el.Element(ns + "RecommendedAction").Value,
                        SecondaryStatusCode = el.Element(ns + "SecondaryStatusCode").Value,
                        SecondaryStatusDescription = el.Element(ns + "SecondaryStatusDescription").Value,
                        SecondaryStatusHitType = el.Element(ns + "SecondaryStatusHitType").Value,
                        SecondaryStatusTitle = el.Element(ns + "SecondaryStatusTitle").Value,
                    };
            }

            // ReSharper restore PossibleNullReferenceException
        }
    }
}