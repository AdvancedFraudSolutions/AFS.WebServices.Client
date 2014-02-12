using System;
using System.Collections.Generic;
using AFS.WebServices.Client.TrueChecks;

namespace AFS.WebServices.Client
{
    /// <summary>
    /// Thrown whenver a request has parameters that are invalid.
    /// </summary>
    [Serializable]
    public class BadRequestException : Exception
    {
        /// <summary>
        /// The validation errors that caused the bad request response.
        /// </summary>
        public Dictionary<string, string[]> Errors { get; private set; }

        public BadRequestException(BadRequestResponse response, Exception inner)
            : base(response.Message, inner)
        {
            Errors = response.ModelState;
        }

        public BadRequestException(BadRequestResponse response)
            : base(response.Message)
        {
            Errors = response.ModelState;
        }
    }
}