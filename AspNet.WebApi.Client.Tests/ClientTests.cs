using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace AspNet.WebApi.Tests
{
    public class ClientTests
    {
        private readonly ClientBase _client;
        private readonly CompareLogic _compareLogic;

        public ClientTests()
        {
            var configuration = new Configuration { Endpoint = new Uri("http://localhost") };

            _client = new Client(configuration) { SetCustomHeadersAction = SetHeadersAction };

            _compareLogic = new CompareLogic();
        }

        private void SetHeadersAction(HttpRequestHeaders headers)
        {
            headers.Add("X-Custom-Header", "Value");
        }

        [Fact]
        public void Configuration()
        {
            Assert.Equal("http://localhost/", _client.Configuration.Endpoint.ToString());
        }

        [Fact]
        public void ProductName()
        {
            Assert.Equal("AspNet.WebApi.Client", _client.ProductName);
        }

        [Fact]
        public void ProductVersion()
        {
            Assert.Equal("1.4.0.0", _client.ProductVersion);
        }

        [Fact]
        public void PrepareRequest()
        {
            var httpClient = new HttpClient();

            _client.PrepareRequest(httpClient, null, null);

            Assert.Equal("AspNet.WebApi.Client/1.4.0.0", httpClient.DefaultRequestHeaders.UserAgent.Single().ToString());
            Assert.Equal("http://localhost/", httpClient.BaseAddress.ToString());
            Assert.Equal("Value", httpClient.DefaultRequestHeaders.GetValues("X-Custom-Header").SingleOrDefault());
        }

        [Fact]
        public void ProcessResponse()
        {
            var httpClient = new HttpClient();
            var response = new HttpResponseMessage();

            _client.ProcessResponse(httpClient, response);

            Assert.True(_compareLogic.Compare(response, response).AreEqual);
        }
    }
}
