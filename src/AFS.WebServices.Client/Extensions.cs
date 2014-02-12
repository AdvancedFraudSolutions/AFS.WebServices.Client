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

            message.EnsureSuccessStatusCode();
        }
    }
}