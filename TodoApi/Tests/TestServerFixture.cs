using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace TodoApi.Tests
{
    public class TestServerFixture : IDisposable
    {

        private readonly TestServer _testServer;
        private readonly HttpClient _httpClient;


        public TestServerFixture()
        {
            var webBuilder = new WebHostBuilder().UseEnvironment("Development").UseStartup<Startup>();
            _testServer = new TestServer(webBuilder);
            _httpClient = _testServer.CreateClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }
    }
}
