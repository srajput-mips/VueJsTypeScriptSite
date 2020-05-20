using BackEnd.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq; 
using Microsoft.Extensions.Options;
using BackEnd.Config;
using BackEnd.Services.Implementations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace BackEnd.Tests
{
    [TestClass]
    public class BackEndTests
    {

        [TestMethod]
        public async Task Test_LiveHttpClient_Pass()
        {

            //assemble 
            string[] ExpectedCatsFemale = { "Garfield", "Simba", "Tabby" };
            string[] ExpectedCatsMale = { "Garfield", "Jim", "Max", "Tom" };
            //mock configuration
            var configMock = Substitute.For<IOptions<ConfigSettings>>();
            configMock.Value.Returns(new ConfigSettings
            {
                CatsBaseURL = "http://agl-developer-test.azurewebsites.net/people.json"
            });
            //mock service
            var mockService = Substitute.For<Cats>(new HttpClient(), configMock);

            //act
            var Result = await mockService.GetCats();

            //assert
            Assert.IsNotNull(Result);
            Assert.AreEqual(Result.Count, 2);
            Assert.IsTrue(Result.Where(x => x.Gender == "Male").First().Names.Any(x => ExpectedCatsMale.Contains(x)));
            Assert.IsTrue(Result.Where(x => x.Gender == "Female").First().Names.Any(x => ExpectedCatsFemale.Contains(x)));
            Console.WriteLine(Result);
        }

        [TestMethod]
        public async Task Test_LiveHttpClient_InvalidURL_fail()
        {

            //assemble 
            //mock configuration
            var configMock = Substitute.For<IOptions<ConfigSettings>>();
            configMock.Value.Returns(new ConfigSettings
            {
                CatsBaseURL = "http://invalidurl.net/people.json"
            });
            //mock service
            var mockService = Substitute.For<Cats>(new HttpClient(), configMock);
            //act
            var Result = await mockService.GetCats();

            //assert 
             
            Assert.IsNull(Result); 
            Console.WriteLine(Result);
        }


        [TestMethod]
        public async Task Test_MockHttpClient()
        {

            //assemble 
            //mock configuration
            var configMock = Substitute.For<IOptions<ConfigSettings>>(); 
            configMock.Value.Returns(new ConfigSettings
            {
                 CatsBaseURL = "http://agl-developer-test.azurewebsites.net/people.json"
            });
            //mock service
            var mockService = Substitute.For<Cats>(MockedHttpClient(), configMock);
            //act
            var Result = await mockService.GetCats();

            //assert
            //assert 
            string[] MockedCatsMale = { "Sandeep", "Rob"};
            Assert.IsNotNull(Result);
            Assert.AreEqual(Result.Count,1);  //fake moked response with only males
            Assert.AreEqual(Result[0].Gender, "Male"); 
            Console.WriteLine(Result);
        }

       
        private HttpClient MockedHttpClient()
        {
            var MockedResponse = "[{'name':'Bob','gender':'Male','age':23,'pets':[{'name':'Sandeep','type':'Cat'},{'name':'Rob','type':'Cat'}]}]";
            var messageHandler = new MockHttpMessageHandler(MockedResponse, HttpStatusCode.OK);
             return new HttpClient(messageHandler); 
        }
  
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;

        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            if (request.Content != null) // Could be a GET-request without a body
            {
                Input = await request.Content.ReadAsStringAsync();
            }
            return new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_response)
            };
        }
    }


}
