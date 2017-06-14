using AFS.WebServices.Client.TrueChecks;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace AFS.WebServices.Client
{
    public interface IAFSClient : IDisposable
    {
        Task<TrueChecksSearchResponse> TrueChecksSearchAsync(TrueChecksSearch query);
        Task<TrueChecksClientSettingsResponse> GetClientTrueChecksSettingsAsync();
        Task PostTrueChecksQueryAction(TrueChecksAction action);
        Task<Image> GetTrueChecksImage(string imagePath);
    }
}