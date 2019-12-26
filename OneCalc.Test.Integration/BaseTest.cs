using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using OneCalc.WebApi;
using Serilog;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OneCalc.Test.Integration
{
    [TestFixture]
    public abstract class BaseTest
    {
        private TestServer _server;

        protected HttpClient _clientExt;
        protected HttpClient _client;


        [OneTimeSetUp]
        public async Task Setup()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseStartup<Startup>();

            // Arrange
            _server = new TestServer(builder);
            _client = _server.CreateClient();
            

            _clientExt = new HttpClient();

            await CleanAsync();
        }

        [OneTimeTearDown]
        public async Task CleanDown()
        {
            await CleanAsync();
        }

        protected virtual async Task CleanAsync()
        {

        }

        protected T GetService<T>()
        {
            try
            {
                return (T)_server.Services.GetService(typeof(T));
            }
            catch (Exception e)
            {

            }

            return default(T);
        }

        protected async Task<T> GetResultGateway<T>(string path, object request, AuthenticationHeaderValue authorization = null) where T : class
        {
            var start = DateTime.Now;

            var stringRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(stringRequest, Encoding.UTF8, "application/json");

            if (authorization != null)
                _client.DefaultRequestHeaders.Authorization = authorization;

            var response = await _client.PostAsync(path, content);

            var responseString = await response.Content.ReadAsStringAsync();

            T result = default(T);

            try
            {
                result = JsonConvert.DeserializeObject<T>(responseString);
            }
            catch
            {
            }

            TestLogging(start, path, request, responseString);

            _client.DefaultRequestHeaders.Authorization = null;
            return result;
        }

        protected async Task<T> GetResultGateway<T>(string path, AuthenticationHeaderValue authorization = null) where T : class
        {
            var start = DateTime.Now;

            if (authorization != null)
                _client.DefaultRequestHeaders.Authorization = authorization;

            var response = await _client.GetAsync(path);

            var responseString = await response.Content.ReadAsStringAsync();

            T result = default(T);

            try
            {
                result = JsonConvert.DeserializeObject<T>(responseString);
            }
            catch
            {
            }

            TestLogging(start, path, "", responseString);

            _client.DefaultRequestHeaders.Authorization = null;

            return result;
        }


        private const string LogFormat = "v: {0} | duration: {1} | requestType: {5}\r\nREQUEST: {2}\r\n{3}\r\n\r\nRESPONSE:\r\n{4}\r\n";

        private void TestLogging(DateTime start, string url, object request, string response)
        {
            var duration = DateTime.Now - start;
            var requestType = request?.GetType().Name;

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            var stringRequest = JsonConvert.SerializeObject(request, settings);

            Debug.WriteLine(LogFormat, "1", duration, url, stringRequest, response, requestType);
        }

        protected string GetUrl(string prevUrl, string currentUrl)
        {
            if (currentUrl.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                return currentUrl;

            var pUrl = new Uri(prevUrl);

            UriBuilder myUri = new UriBuilder(pUrl.Scheme, pUrl.Host);
            myUri.Path = currentUrl;

            return myUri.ToString();
        }
    }
}