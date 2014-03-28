using System;
using System.Drawing;
using System.IO;
using AFS.WebServices.Client.TrueChecks;
using NUnit.Framework;

namespace AFS.WebServices.Client.Tests
{
    public class Tests
    {
        private static AFSClient GetAFSClient()
        {
            var apikey = File.ReadAllText(@"c:\afs.webservices.client.tests.apikey.txt");
            return new AFSClient(apikey, "http://localhost:55514/");
        }

        [Test]
        public void Can_Do_TrueChecks_Search()
        {
            var client = GetAFSClient();
            var results = client.TrueChecksSearch(new TrueChecksSearch
            {
                BranchId = "ctx",
                TellerId="roverby",
                RoutingNumber="123412342",
                AccountNumber="1287182239843",
                CheckAmount=123.45m,
                CheckNumber = "1234",
                DoDepositChekSearch=true,
                DoTrueChecksSearch = true,
                IsBusinessOrPersonalCheck = true,
                Maker = "Some Guy",
            });

            Assert.NotNull(results);
            Assert.NotNull(results.DepositChekResults);
        }

        [Test]
        public void Can_Post_TrueChecks_Action()
        {
            var client = GetAFSClient();
            var results = client.TrueChecksSearch(new TrueChecksSearch
            {
                BranchId = "ctx",
                TellerId="roverby",
                RoutingNumber="123412342",
                AccountNumber="1287182239843",
                CheckAmount=123.45m,
                CheckNumber = "1234",
                DoDepositChekSearch=true,
                DoTrueChecksSearch = true,
                IsBusinessOrPersonalCheck = true,
                Maker = "Some Guy",
            });

            client.PostTrueChecksQueryAction(new TrueChecksAction
            {
                QueryId =  results.QueryId,
                Action = QueryAction.RefinedSearch
            });
        }

        [Test]
        public void Can_Handle_Bad_TrueChecks_Search()
        {
            var client = GetAFSClient();
            try
            {
                client.TrueChecksSearch(new TrueChecksSearch());
                Assert.Fail("TrueChecksSearch did not throw an exception");
            }
            catch (BadRequestException ex)
            {
                Assert.IsNotEmpty(ex.Message);
                Assert.Greater(ex.Errors.Count, 0);
            }
        }

        [Test]
        public void Can_Get_TrueChecks_ClientSettings()
        {
            var client = GetAFSClient();
            var settings = client.GetClientTrueChecksSettings();
            Assert.IsNotNull(settings);
        }

        [Test]
        public void Can_Get_TrueChecks_Image()
        {
            var client = GetAFSClient();

            using (var img = client.GetTrueChecksImage("00b92e4b6ea346ec8734f68a9e1fbcf4.tif"))
                Assert.NotNull(img);
        }

    }
}
