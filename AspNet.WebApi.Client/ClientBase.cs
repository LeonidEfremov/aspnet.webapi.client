using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace AspNet.WebApi
{
    public class ClientBase
    {
        public readonly Configuration Configuration;
        public readonly string ProductName = $"{Assembly.GetExecutingAssembly().GetName().Name}";
        public readonly string ProductVersion = $"{Assembly.GetExecutingAssembly().GetName().Version}";

        public Action<HttpRequestHeaders> SetCustomHeadersAction { get; set; }

        public ClientBase(Configuration configuration)
        {
            Configuration = configuration;
            SetCustomHeadersAction = (headers) => { };
        }

        public virtual void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            var headers = client.DefaultRequestHeaders;

            SetCustomHeadersAction(headers);

            headers.UserAgent.Clear();
            headers.UserAgent.Add(new ProductInfoHeaderValue(ProductName, ProductVersion));

            if (client.BaseAddress == null)
            {
                client.BaseAddress = Configuration.Endpoint;
            }
        }

        public virtual void ProcessResponse(HttpClient client, HttpResponseMessage response) { }
    }
}