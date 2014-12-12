using System.Net.Http;

namespace AFS.WebServices.Client.Example
{
    using System;
    using System.IO;
    using System.Linq;
    using TrueChecks;

    internal class Program
    {
        private static void Main()
        {
            // put your api key in the connection string defined in app.config
            // to avoid entering it at run-time
            var connectionString = ConnectionStrings.AFSApi ??
                                   PromptForConnectionString();


            // first create an AFSClient object to communicate with the web service
            // you can create the client with a connection string or with the url/api key
            // the connection string should look like:
            //      Url = https://api.advancedfraudsolutions.com; ApiKey = yourClientApiKeyGoesHere
            var client = new AFSClient(connectionString);
            int? refinedQueryId = null;
            do
            {
                try
                {
                    // before displaying the search interface to the end-user
                    // you'll want to get the settings that control depositor searching
                    var settings = client.GetClientTrueChecksSettings();
                    Print(settings);

                    // get the user's TrueChecks query
                    var trueChecksQuery = GetSearchQuery(settings, refinedQueryId);

                    // get the results for the query
                    var searchResults = client.TrueChecksSearch(trueChecksQuery);
                    Print(searchResults);

                    // the search result will have zero to many image references
                    // you can download the images to display to the user
                    // in this example, we'll just save any images to a folder
                    SaveImages(client, searchResults);

                    // finally, the end user will inform the TrueChecks service
                    // of what action they have decided to take with the check
                    var action = GetCheckAction(searchResults);
                    client.PostTrueChecksQueryAction(action);

                    // if the end-user is refining their search, you should 
                    // include the original query id in the next search
                    refinedQueryId = action.Action == QueryAction.RefinedSearch
                        ? searchResults.QueryId
                        : (int?) null;
                }
                catch (BadRequestException ex)
                {
                    // an invalid service request causes a BadRequestException
                    // to be thrown (the http response code is 400 BadRequest)
                    // check ex.Errors to see what you (or the user) did wrong
                    Print(ex);
                }
                catch (HttpRequestException ex)
                {
                    // an HttpRequestException is thrown for all other 
                    // non successful responses (401 Unauthorized, 403 Forbidden,
                    // 404 NotFound, 500 InternalServerError, etc.)
                    // these represent error conditions that are unrelated to
                    // end-user input
                    Print(ex);
                }
            } while ("Would you like to continue [y/n]? ".Prompt(Parse.Bool));

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string PromptForConnectionString()
        {
            var apiKey = "API Key: ".Prompt();
            return string.Format("ApiKey={0}", apiKey);
        }


        private static TrueChecksAction GetCheckAction(TrueChecksSearchResponse results)
        {
            PrintHeading("Record Action Taken For Check");

            var action = new TrueChecksAction
            {
                QueryId = results.QueryId,
                Action = string.Format("Action Taken [{0}]: ", typeof(QueryAction).GetEnumChoices())
                    .Prompt(Parse.Enum<QueryAction>)
            };

            return action;
        }

        private static void SaveImages(AFSClient client, TrueChecksSearchResponse searchResults)
        {
            PrintHeading("Saving Images");
            var imgDir = AppSettings.ImageDirectoryPath;
            if (!imgDir.Exists) imgDir.Create();

            // get all image references
            var imgPaths = searchResults.CheckAlertResults.Select(x => x.FrontImage)
                .Concat(searchResults.PersonResults.Select(x => x.Image))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            // download image and save to disk
            foreach (var imgPath in imgPaths)
            {
                using (var img = client.GetTrueChecksImage(imgPath))
                {
                    var file = new FileInfo(Path.Combine(imgDir.FullName, imgPath));
                    if (file.Exists) file.Delete();
                    file.Directory.Create();
                    img.Save(file.FullName);
                }
            }

            Console.WriteLine("{0} images saved in {1}", imgPaths.Length, imgDir.FullName);
        }

        private static TrueChecksSearch GetSearchQuery(TrueChecksClientSettingsResponse settings, int? refinedQueryId)
        {
            PrintHeading("Enter query information:");

            var query = new TrueChecksSearch
            {
                RefinedQueryId = refinedQueryId,

                // always include some branch/teller information for reporting
                BranchId = "Branch ID: ".Prompt(),
                TellerId = "Teller ID: ".Prompt(),

                // always get the check information
                IsBusinessOrPersonalCheck = "Is the check a business or personal check [y/n]? ".Prompt(Parse.Bool),
                RoutingNumber = "Routing Number: ".Prompt(),
                AccountNumber = "Account Number: ".Prompt(),
                CheckNumber = "Check Number: ".Prompt(),
                CheckAmount = "Check Amount: $".Prompt(decimal.Parse),
                Maker = "Maker: ".Prompt(),
            };

            // optionally, you could control what search takes place based on some criteria
            query.DoTrueChecksSearch = query.CheckAmount >= 10;
            query.DoDepositChekSearch = query.CheckAmount >= 25;

            // get the depositor information when the customer allows/requires it
            if (settings.DepositorSearchEnabled)
            {
                query.FirstName = "Depositor First Name: ".Prompt();
                query.LastName = "Depositor Last Name: ".Prompt();

                if (settings.SSNMode != FieldMode.Disabled)
                    query.SSN = "Depositor SSN: ".Prompt();

                if (settings.FIIDMode != FieldMode.Disabled)
                    query.FIID = "Depositor Financial Instituion ID: ".Prompt();

                if (settings.DriversLicenseMode == FieldMode.Disabled) return query;
                query.DriversLicenseState = "Depositor Driver's Lic. State: ".Prompt();
                query.DriversLicenseNumber = "Depositor Driver's Lic. #: ".Prompt();
            }

            return query;
        }

        private static void Print(TrueChecksSearchResponse results)
        {
            PrintHeading("TrueChecks Search Results");
            Console.WriteLine("QueryId: {0}", results.QueryId);
            Console.WriteLine("OverallRecommendedAction: {0}", results.OverallRecommendedAction);
            Console.WriteLine("OverallRecommendedActionId: {0}", results.OverallRecommendedActionId);

            // show the TrueChecks check alert results
            Console.WriteLine("CheckAlertResults: {0}", results.CheckAlertResults.Length);
            Indent++;
            foreach (var r in results.CheckAlertResults
                .OrderByDescending(x => x.RecommendedActionWeight)
                .ThenByDescending(x => x.UpdatedDate))
            {
                PrintIndented("FraudType: {0}", r.FraudType);
                PrintIndented("RecommendedAction: {0}", r.RecommendedAction);
                PrintIndented("RecommendedActionId: {0}", r.RecommendedActionId);
                PrintIndented("Notes: {0}", r.Notes);
                PrintIndented("CreatedDate: {0}", r.CreatedDate);
                PrintIndented("UpdatedDate: {0}", r.UpdatedDate);
                PrintIndented("CheckType: {0}", r.CheckType);
                PrintIndented("RoutingNumber: {0}", r.RoutingNumber);
                PrintIndented("AccountNumber: {0}", r.AccountNumber);
                PrintIndented("CheckNumber: {0}", r.CheckNumber);
                PrintIndented("Amount: {0}", r.Amount);
                PrintIndented("FrontImage: {0}", r.FrontImage);
                PrintIndented("MakerName: {0}", r.MakerName);
                PrintIndented("MakerAddress: {0}", r.MakerAddress);
                PrintHorizontalLine();
            }

            // print person results
            Console.WriteLine("PersonResults: {0}", results.PersonResults.Length);
            foreach (var r in results.PersonResults.OrderBy(x => x.NameMatchOnly).ThenByDescending(x => x.PersonUpdated)
                )
            {
                PrintIndented("Name: {1}, {0}", r.FirstName, r.LastName);
                PrintIndented("SSN: {0}", r.SSN);
                PrintIndented("Financial Institution ID: {0}", r.FIID);
                PrintIndented("DriversLicense: {0} {1}", r.DriversLicenseState, r.DriversLicense);
                PrintIndented("Updated: {0}", r.PersonUpdated);
                PrintIndented("Image: {0}", r.Image);
                PrintIndented("Activity Summary:");
                Indent++;
                foreach (var item in r.ActivitySummary)
                    PrintIndented("{0}: {1}", item.Key, item.Value);
                Indent--;
                PrintHorizontalLine();
            }

            // print ews results
            Console.WriteLine("AccountStatusResults:");
            if (results.IsEWSDepositChekServiceDown)
            {
                PrintIndented("EWS DepositChek service is down.");
            }
            else if(results.DepositChekResults != null)
            {
                var r = results.DepositChekResults;
                PrintIndented("RecommendedAction: {0}", r.RecommendedAction);
                PrintIndented("RecommendedActionId: {0}", r.RecommendedActionId);

                PrintIndented("PrimaryStatusCode: {0}", r.PrimaryStatusCode);
                PrintIndented("PrimaryStatusDescription: {0}", r.PrimaryStatusDescription);
                PrintIndented("PrimaryStatusHitType: {0}", r.PrimaryStatusHitType);
                PrintIndented("PrimaryStatusShortTitle: {0}", r.PrimaryStatusShortTitle);
                PrintIndented("PrimaryStatusTitle: {0}", r.PrimaryStatusTitle);

                PrintIndented("SecondaryStatusCode: {0}", r.SecondaryStatusCode);
                PrintIndented("SecondaryStatusDescription: {0}", r.SecondaryStatusDescription);
                PrintIndented("SecondaryStatusHitType: {0}", r.SecondaryStatusHitType);
                PrintIndented("SecondaryStatusTitle: {0}", r.SecondaryStatusTitle);

                PrintIndented("AdditionalStatusCode: {0}", r.AdditionalStatusCode);
                PrintIndented("AdditionalStatusDescription: {0}", r.AdditionalStatusDescription);
                PrintIndented("AdditionalStatusHitType: {0}", r.AdditionalStatusHitType);
                PrintIndented("AdditionalStatusTitle: {0}", r.AdditionalStatusTitle);

                PrintIndented("PreviousStatusCode: {0}", r.PreviousStatusCode);
                PrintIndented("PreviousStatusDescription: {0}", r.PreviousStatusDescription);
                PrintIndented("PreviousStatusHitType: {0}", r.PreviousStatusHitType);
                PrintIndented("PreviousStatusTitle: {0}", r.PreviousStatusTitle);

                PrintIndented("LastReturnReasonCode: {0}", r.LastReturnReasonCode);
                PrintIndented("LastReturnReasonDescription: {0}", r.LastReturnReasonDescription);
                PrintIndented("LastReturnReasonHitType: {0}", r.LastReturnReasonHitType);
                PrintIndented("LastReturnReasonTitle: {0}", r.LastReturnReasonTitle);

                PrintIndented("ErrorDescription: {0}", r.ErrorDescription);
                PrintIndented("ErrorTitle: {0}", r.ErrorTitle);
            }
            Indent--;
        }

        private static void Print(TrueChecksClientSettingsResponse settings)
        {
            PrintHeading("Client's TrueChecks Settings");
            Console.WriteLine("DepositorSearchEnabled: {0}", settings.DepositorSearchEnabled);
            Console.WriteLine("SSNMode: {0}", settings.SSNMode);
            Console.WriteLine("FIIDMode: {0}", settings.FIIDMode);
            Console.WriteLine("DriversLicenseMode: {0}", settings.DriversLicenseMode);
        }

        private static void Print(HttpRequestException ex)
        {
            PrintHeading("HttpRequestException", ConsoleColor.Red);
            Console.WriteLine(ex.Message);
        }

        private static void Print(BadRequestException ex)
        {
            PrintHeading("BadRequestException: " + ex.Message, ConsoleColor.Red);

            foreach (var error in ex.Errors)
            {
                // this is the property or field that had a problem
                Console.WriteLine(error.Key);

                // these are the problems
                Indent++;
                PrintIndented(error.Value);
                Indent--;
            }
        }

        private static void PrintHeading(string heading, ConsoleColor color = ConsoleColor.Green)
        {
            Console.WriteLine();
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(heading);
            Console.ForegroundColor = oldColor;
        }

        private static void PrintIndented(string format, params object[] args)
        {
            if (Indent < 1) Indent = 1;
            var indention = string.Concat(Enumerable.Range(0, Indent).Select(x => "   "));
            Console.WriteLine(indention + format, args);
        }

        public static byte Indent { get; set; }

        private static void PrintHorizontalLine()
        {
            Console.WriteLine(new string('_', Math.Min(Console.BufferWidth, Console.WindowWidth)));
        }
    }


}