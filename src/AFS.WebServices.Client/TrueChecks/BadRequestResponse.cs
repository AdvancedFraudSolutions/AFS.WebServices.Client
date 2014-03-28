using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AFS.WebServices.Client.TrueChecks
{

    /// <summary>
    /// Information about the bad request.
    /// </summary>
    public class BadRequestResponse : IDeserializeFromResponseStream
    {
        /// <summary>
        /// A message describing why a bad request status was returned.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A collection of error messages.
        /// </summary>
        public Dictionary<string, string> Errors { get; set; }

        public void DeserializeFromResponseStream(Stream stream, string contentType)
        {
            this.EnsureContentTypeStartsWith(contentType, "application/xml");

            XElement xml;

            using (var reader = new StreamReader(stream))
                xml = XElement.Load(reader);

            Message = xml.Element("Message").Value;

            Errors = xml.Element("ModelState").Elements().Select(el =>
                new KeyValuePair<string, string>(el.Name.LocalName, el.Value))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
