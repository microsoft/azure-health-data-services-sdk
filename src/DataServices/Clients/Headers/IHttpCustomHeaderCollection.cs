﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

namespace DataServices.Clients.Headers
{
    /// <summary>
    /// Interface that defines how headers from a request can be obtained from the collection of custom headers.
    /// </summary>
    public interface IHttpCustomHeaderCollection : IList<IHeaderNameValuePair>
    {
        /// <summary>
        /// Appends and replaces existing headers with custom headers and returns the modified collection headers.
        /// </summary>
        /// <param name="request">Http request message.</param>
        /// <param name="headers">Collection of custom headers.</param>
        /// <returns>Modified collection headers</returns>
        NameValueCollection AppendAndReplace(HttpRequestMessage request);
    }
}
