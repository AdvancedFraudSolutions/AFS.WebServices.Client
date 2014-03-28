using System;
using System.Data.Common;
using System.Drawing;
using System.Threading;
using AFS.WebServices.Client.TrueChecks;

namespace AFS.WebServices.Client
{
    /// <summary>
    /// Provides methods for interacting with the Advanced Fraud Solutions web service.
    /// </summary>
    public class AFSClient
    {
        private readonly HttpClient _httpClient;


        /// <param name="baseAddress">The base URL of the web service. If null the default base address will be used.</param>
        /// <param name="apiKey">Your API key that is used to identify the integrator and customer.</param>
        public AFSClient(string apiKey, string baseAddress)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");

            _httpClient = CreateHttpClient(baseAddress ?? Urls.DefaultBaseAddress, apiKey);
        }

        /// <param name="connectionString">A connection string containing the information needed to connect to the AFS web services.</param>
        public AFSClient(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");

            var csb = new DbConnectionStringBuilder { ConnectionString = connectionString };

            object baseAddress, apiKey;
            if (!csb.TryGetValue("apikey", out apiKey))
                BadConnectionString();

            if (!csb.TryGetValue("url", out baseAddress))
                baseAddress = Urls.DefaultBaseAddress;

            _httpClient = CreateHttpClient((string)baseAddress, (string)apiKey);
        }

        private static HttpClient CreateHttpClient(string baseAddress, string apiKey)
        {
            return new HttpClient(baseAddress)
            {
                AuthorizationHeader = string.Format("{0} {1}", Constants.XApiKey, apiKey),
                RequestContentType = "application/x-www-form-urlencoded",
                RequestAccept = "application/xml"
            };
        }

        private static void BadConnectionString(Exception innerException = null)
        {
            throw new FormatException(
                string.Format(
                    "The connection string is invalid. Example connection string: 'Url = {0}; ApiKey = w91tSmogcsncMfebSb8ypzxe65G8JLef'",
                    Urls.DefaultBaseAddress), innerException);
        }

        public TrueChecksSearchResponse TrueChecksSearch(TrueChecksSearch query)
        {
            if (query == null) throw new ArgumentNullException("query");
            var result = _httpClient.Post<TrueChecksSearchResponse>(Urls.TrueChecksSearch, query);
            return result;
        }

        public TrueChecksClientSettingsResponse GetClientTrueChecksSettings()
        {
            return _httpClient.Get<TrueChecksClientSettingsResponse>(Urls.TrueChecksClientSettings);
        }

        public void PostTrueChecksQueryAction(TrueChecksAction action)
        {
            _httpClient.Post(Urls.TrueChecksCheckAction, action);
        }

        public Image GetTrueChecksImage(string imagePath)
        {
            var url = Urls.TrueChecksImages(imagePath);

            var image = _httpClient.Get(url, resp =>
            {
                using (var stream = resp.GetResponseStream())
                    return Image.FromStream(stream);
            });

            return image;
        }
    }
}