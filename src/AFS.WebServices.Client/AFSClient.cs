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
    public class AFSClient : IAFSClient
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
        public static AFSClient CreateFromConnectionString(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");

            var csb = new DbConnectionStringBuilder { ConnectionString = connectionString };

            object baseAddress, apiKey;
            if (!csb.TryGetValue("apikey", out apiKey))
                BadConnectionString();

            if (!csb.TryGetValue("url", out baseAddress))
                baseAddress = Urls.DefaultBaseAddress;

            var httpClient = CreateHttpClient((string) baseAddress, (string) apiKey);
            return new AFSClient(httpClient);
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
        public Task<TrueChecksSearchResponse> TrueChecksSearchAsync(TrueChecksSearch query)
        {
            if (query == null) throw new ArgumentNullException("query");

            var postTask = _client.PostAsJsonAsync(Urls.TrueChecksSearch, query, CancellationToken);

            var readTask = postTask.ContinueWith(t =>
            {
                using (var response = t.Result)
                {
                    t.Result.EnsureGoodResponse(CancellationToken);
                    return TaskUtil.ReadAsAsync<TrueChecksSearchResponse>(response.Content, CancellationToken).Result;
                }

            }, CancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);

            return readTask;
        }

        public Task<TrueChecksClientSettingsResponse> GetClientTrueChecksSettingsAsync()
        {
            var getTask = _client.GetAsync(Urls.TrueChecksClientSettings, CancellationToken);

            var readTask = getTask.ContinueWith(t =>
            {
                using (var response = t.Result)
                {
                    response.EnsureGoodResponse(CancellationToken);
                    return TaskUtil.ReadAsAsync<TrueChecksClientSettingsResponse>(response.Content, CancellationToken)
                        .Result;
                }

            }, CancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);

            return readTask;
        }

        public Task PostTrueChecksQueryAction(TrueChecksAction action)
        {
            var postTask = _client.PostAsJsonAsync(Urls.TrueChecksCheckAction, action, CancellationToken);

            var readTask = postTask.ContinueWith(t =>
            {
                using (var response = t.Result)
                {
                    response.EnsureGoodResponse(CancellationToken);
                }

            }, CancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);

            return readTask;

        }

        public  Task<Image> GetTrueChecksImage(string imagePath)
        {
            var url = Urls.TrueChecksImages(imagePath);
            var getTask = _client.GetAsync(url, CancellationToken);


            var readTask = getTask.ContinueWith(t =>
            {
                using (var response = t.Result)
                {
                    response.EnsureGoodResponse(CancellationToken);

                    using (var stream = response.Content.ReadAsStreamAsync().Result)
                        return Image.FromStream(stream);
                }

            }, CancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);

            return readTask;

        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}