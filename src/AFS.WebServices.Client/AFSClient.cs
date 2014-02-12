using System;
using System.Data.Common;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AFS.WebServices.Client.TrueChecks;

namespace AFS.WebServices.Client
{
    /// <summary>
    /// Provides methods for interacting with the Advanced Fraud Solutions web service.
    /// </summary>
    public class AFSClient : IDisposable
    {
        private readonly HttpClient _client;
        public CancellationToken CancellationToken { get; set; }

        public AFSClient(HttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            _client = httpClient;
        }

        /// <param name="baseAddress">The base URL of the web service.</param>
        /// <param name="apiKey">Your API key that is used to identify the integrator and customer.</param>
        public AFSClient(string apiKey, string baseAddress = Urls.DefaultBaseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException("baseAddress");
            if (apiKey == null) throw new ArgumentNullException("apiKey");

            _client = CreateHttpClient(baseAddress, apiKey);
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

            _client = CreateHttpClient((string)baseAddress, (string)apiKey);
        }

        private static void BadConnectionString(Exception innerException = null)
        {
            throw new FormatException(
                string.Format(
                    "The connection string is invalid. Example connection string: 'Url = {0}; ApiKey = w91tSmogcsncMfebSb8ypzxe65G8JLef'",
                    Urls.DefaultBaseAddress), innerException);
        }

        private static HttpClient CreateHttpClient(string baseAddress, string apiKey)
        {
            var client = new HttpClient { BaseAddress = new Uri(baseAddress) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.XApiKey, apiKey);
            return client;
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<TrueChecksSearchResponse> TrueChecksSearchAsync(TrueChecksSearch query)
        {
            if (query == null) throw new ArgumentNullException("query");

            using (var response = await _client.PostAsJsonAsync(Urls.TrueChecksSearch, query, CancellationToken))
            {
                await response.HandleBadRequest(CancellationToken);

#if NET40
                var results = await response.Content.ReadAsAsync<TrueChecksSearchResponse>();
#else
                var results = await response.Content.ReadAsAsync<TrueChecksSearchResponse>(CancellationToken);
#endif
                return results;
            }
        }

        public async Task<TrueChecksClientSettingsResponse> GetClientTrueChecksSettingsAsync()
        {
            using (var response = await _client.GetAsync(Urls.TrueChecksClientSettings, CancellationToken))
            {
                await response.HandleBadRequest(CancellationToken);
#if NET40
                var results = await response.Content.ReadAsAsync<TrueChecksClientSettingsResponse>();
#else
                var results = await response.Content.ReadAsAsync<TrueChecksClientSettingsResponse>(CancellationToken);
#endif
                return results;
            }
        }

        public async Task PostTrueChecksQueryAction(TrueChecksAction action)
        {
            using (var response = await _client.PostAsJsonAsync(Urls.TrueChecksCheckAction, action, CancellationToken))
                await response.HandleBadRequest(CancellationToken);
        }

        public async Task<Image> GetTrueChecksImage(string imagePath)
        {
            var url = Urls.TrueChecksImages(imagePath);
            using (var response = await _client.GetAsync(url, CancellationToken))
            {
                await response.HandleBadRequest(CancellationToken);
                using (var stream = await response.Content.ReadAsStreamAsync())
                    return Image.FromStream(stream);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}