using System.IO;

namespace AFS.WebServices.Client
{
    internal interface IDeserializeFromResponseStream
    {
        void DeserializeFromResponseStream(Stream stream, string contentType);
    }
}