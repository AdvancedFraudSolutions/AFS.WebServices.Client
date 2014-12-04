using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AFS.WebServices.Client.TrueChecks;

namespace AFS.WebServices.Client
{
    public static class Extensions
    {
        public static async Task HandleBadRequest(this HttpResponseMessage message, CancellationToken cancellationToken)
        {
            if (message.IsSuccessStatusCode)
                return;

            if (message.StatusCode == HttpStatusCode.BadRequest)
            {
#if NET40
                var errors = await message.Content.ReadAsAsync<BadRequestResponse>();
#else
                var errors = await message.Content.ReadAsAsync<BadRequestResponse>(cancellationToken);
#endif
                throw new BadRequestException(errors);
            }

            try
            {
                message.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                ex.Data[Constants.StatusCode] = message.StatusCode;
                ex.Data[Constants.ReasonPhrase] = message.ReasonPhrase;
                throw;
            }
        }

        public static HttpStatusCode? GetStatusCode(this HttpRequestException ex)
        {
            if (ex.Data.Contains(Constants.StatusCode))
                return (HttpStatusCode) ex.Data[Constants.StatusCode];

            return null;
        }

        public static string GetReasonPhrase(this HttpRequestException ex)
        {
            if (ex.Data.Contains(Constants.ReasonPhrase))
                return (string) ex.Data[Constants.ReasonPhrase];

            return null;
        }
    }
}