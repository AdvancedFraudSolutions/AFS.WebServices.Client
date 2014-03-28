using System;
using System.Net;
using AFS.WebServices.Client.TrueChecks;

namespace AFS.WebServices.Client
{
    internal class HttpClient
    {
        private readonly Uri _baseUri;

        public HttpClient(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException("baseAddress");

            _baseUri = new Uri(baseAddress);
        }

        public string AuthorizationHeader { get; set; }
        public string RequestContentType { get; set; }
        public string RequestAccept { get; set; }

        public TResponse Post<TResponse>(string relativeUrl, ISerializeToRequestStream requestBody) where TResponse : IDeserializeFromResponseStream
        {
            return Post(relativeUrl, requestBody, resp =>
            {
                using (var stream = resp.GetResponseStream())
                {
                    var result = Activator.CreateInstance<TResponse>();
                    result.DeserializeFromResponseStream(stream, resp.ContentType);
                    return result;
                }
            });
        }

        public TResponse Post<TResponse>(string relativeUrl, ISerializeToRequestStream requestBody, Func<HttpWebResponse,TResponse> factory)
        {
            var req = CreateRequest("POST", relativeUrl);
            using (var stream = req.GetRequestStream())
                requestBody.SerializeToRequestStream(stream, RequestContentType);

            try
            {
                using (var resp = (HttpWebResponse) req.GetResponse())
                    return factory(resp);
            }
            catch (WebException ex)
            {
                throw CreateBadRequestException(ex);
            }
        }

        public void Post(string relativeUrl, ISerializeToRequestStream requestBody)
        {
            var req = CreateRequest("POST", relativeUrl);
            using (var stream = req.GetRequestStream())
                requestBody.SerializeToRequestStream(stream, RequestContentType);

            try
            {
                using (req.GetResponse())
                {
                }
            }
            catch (WebException ex)
            {
                throw CreateBadRequestException(ex);
            }
        }

        public TResponse Get<TResponse>(string relativeUrl) where TResponse : IDeserializeFromResponseStream
        {
            return Get(relativeUrl, resp =>
            {
                using (var stream = resp.GetResponseStream())
                {
                    var result = Activator.CreateInstance<TResponse>();
                    result.DeserializeFromResponseStream(stream, resp.ContentType);
                    return result;
                }
            });
        }

        public TResponse Get<TResponse>(string relativeUrl, Func<HttpWebResponse,TResponse> factory)
        {
            var req = CreateRequest("GET", relativeUrl);

            try
            {
                using (var resp = (HttpWebResponse) req.GetResponse())
                    return factory(resp);
            }
            catch (WebException ex)
            {
                throw CreateBadRequestException(ex);
            }
        }

        private static BadRequestException CreateBadRequestException(WebException webException)
        {
            using (webException.Response)
            using (var stream = webException.Response.GetResponseStream())
            {
                var badResponse = new BadRequestResponse();
                badResponse.DeserializeFromResponseStream(stream, webException.Response.ContentType);
                throw new BadRequestException(badResponse, webException);
            }
        }

        private HttpWebRequest CreateRequest(string method, string relativeUrl)
        {
            if (method == null) throw new ArgumentNullException("method");
            var uri = new Uri(_baseUri, relativeUrl);

            // ReSharper disable once AccessToStaticMemberViaDerivedType
            var req = (HttpWebRequest)HttpWebRequest.Create(uri);
            req.Method = method;

            if (AuthorizationHeader != null)
                req.Headers[HttpRequestHeader.Authorization] = AuthorizationHeader;

            if (RequestContentType != null)
                req.ContentType = RequestContentType;

            if (RequestAccept != null)
                req.Accept = RequestAccept;

            return req;
        }
    }
}
