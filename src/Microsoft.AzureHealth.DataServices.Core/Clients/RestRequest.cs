﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.AzureHealth.DataServices.Clients
{
    /// <summary>
    /// Makes an HTTP request to a web server.
    /// </summary>
    public class RestRequest
    {
        /// <summary>
        /// Creates an instance of the RestRequest.
        /// </summary>
        /// <param name="builder">REST request builder that creates the HttpWebRequest object.</param>
        /// <param name="httpClientFactory">IHttpClientFactory to create HTTPClient object.</param>
        /// <param name="logger">Optional logger.</param>
        public RestRequest(RestRequestBuilder builder, IHttpClientFactory httpClientFactory = null, ILogger logger = null)
           : this(logger)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            this.httpClientFactory = httpClientFactory;
            this.builder = builder;
        }

        /// <summary>
        /// Creates an instance of RestRequest.
        /// </summary>
        /// <param name="logger">Optional ILogger.</param>
        protected RestRequest(ILogger logger = null)
        {
            this.logger = logger;
        }

        private readonly ILogger logger;
        private readonly RestRequestBuilder builder;
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Sends and http request and returns a response.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> SendAsync()
        {
            try
            {
                HttpClient client;
                client = httpClientFactory == null ? new HttpClient() : httpClientFactory.CreateClient();
                HttpRequestMessage message = builder.Build();
                if (builder.Certificate != null)
                {
                    HttpClientHandler handler = new();
                    handler.ClientCertificates.Add(builder.Certificate);
                    client = new HttpClient(handler);
                }
                HttpResponseMessage response = await client.SendAsync(message);
                logger?.LogInformation("Rest response returned status {StatusCode}.", response.StatusCode);
                logger?.LogTrace("Rest response returned with content-type {ContentType}.", response.Content?.Headers.ContentType);

                if (response.IsSuccessStatusCode)
                {
                    logger?.LogInformation("Return http response.");
                }
                else
                {
                    logger?.LogWarning("Rest response returned fault reason phrase {ReasonPhrase}.", response.ReasonPhrase);
                }

                return response;
            }
            catch (WebException wex)
            {
                logger?.LogError(wex, "Rest web request faulted '{Status}'.", wex.Status);
                throw;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Rest request faulted '{Message}'.", ex.Message);
                throw;
            }
        }
    }
}
