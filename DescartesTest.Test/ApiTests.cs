using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            var json = JObject.Parse(responseContent);

            var actual = json.GetValue("diffResultType");

            Assert.IsNotNull(responseContent);

            string expectedResult = "Equals";

            Assert.AreEqual(expectedResult, actual);

        }
    }
}