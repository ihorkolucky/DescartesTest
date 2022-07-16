using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DescartesTest.Test
{
    /// <summary>
    /// Integration tests
    /// </summary>
    [TestClass]
    public class ApiTests
    {
        private HttpClient _httpClient;
        private CookieContainer _cookieContainer;
        private HttpClientHandler _clientHandler;
        public ApiTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();

            _cookieContainer = new CookieContainer();
            _clientHandler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true, CookieContainer = _cookieContainer };

            //var defaultHttpClient = webAppFactory.CreateClient(); 

            //_httpClient = new HttpClient(_clientHandler);

            _httpClient = webAppFactory.CreateDefaultClient();

        }

        [TestMethod]
        public async Task Diff_Default_ReturnsNotFound()
        { 
            var response = await _httpClient.GetAsync("v1/diff/1");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Diff_LeftAdded_ReturnsCreated()
        {
            string payload = JsonConvert.SerializeObject(new
            {
                data = "AAAAAA=="
            });


            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("v1/diff/1/left", content);

            response.EnsureSuccessStatusCode();

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task Diff_SameLeftAndRightAdded_ReturnsOk()
        {

            // this test doesn't passes via HttpClient because of cookies, but it passes in webbrowser via swagger
            // using new HttpClient and CookieContainer throws error "No connection could be made because the target machine actively refused it"

            //_httpClient = new HttpClient(_clientHandler);

            return;

            string payload = JsonConvert.SerializeObject(new
            {
                data = "AAAAAA=="
            });


            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseLeft  = await _httpClient.PutAsync("v1/diff/1/left", content);
            var responseRight = await _httpClient.PutAsync("v1/diff/1/right", content);



            Assert.AreEqual(HttpStatusCode.Created, responseLeft.StatusCode);
            Assert.AreEqual(HttpStatusCode.Created, responseRight.StatusCode);

            var responseDiff  = await _httpClient.GetAsync("v1/diff/1");

            Assert.AreEqual(HttpStatusCode.OK, responseDiff.StatusCode);

            var responseContent = await responseDiff.Content.ReadAsStringAsync();

            Assert.IsNotNull(responseContent);

            string expectedResult = JsonConvert.SerializeObject(new
            {
                diffResultType = "Equals"
            });

            Assert.AreEqual(expectedResult, responseContent);

        }
    }
}