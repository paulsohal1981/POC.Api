using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using POC.Api;
using System;
using System.Net.Http;
using Xunit;

namespace POC.ApiIntegrationTest
{
    public class BaseTest : IDisposable
    {
        #region protected members

        protected readonly TestServer _server;
        protected readonly HttpClient _client;

        #endregion


        #region ctor

        public BaseTest()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder()
            .UseStartup<Startup>()
            .UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        #endregion

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }

}
