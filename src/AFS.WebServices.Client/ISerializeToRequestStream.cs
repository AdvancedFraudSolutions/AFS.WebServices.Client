using System.IO;

namespace AFS.WebServices.Client
{
    internal interface ISerializeToRequestStream
    {
        void SerializeToRequestStream(Stream stream, string contentType);
    }
}