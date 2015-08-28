using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model.Requests;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class RestSharpRequestHandlerTests
    {
        [Fact]
        public void BuildRequestParamsHandlesBasicObject()
        {
            var subject = new RestSharpRequestHandler("xxx");
            var testParams = new TestParams
            {
                MyString = "foo",
                MyBool = true,
                MyBytes = new byte[10],
                MyEnum = TestEnumParams.TestItem2,
                MyInt = 90
            };

            var result = subject.BuildRequestParams(testParams);

            Assert.Equal(4, result.Keys.Count);
            Assert.Equal("foo", result["my_string"]);
            Assert.Equal("true", result["my_bool"]);
            Assert.Equal("test_item2", result["my_enum"]);
            Assert.Equal("90", result["my_int"]);
        }

        [Fact]
        public void BuildRequestParamsHandlesComplexObject()
        {
            var complexObject = new TestParams
            {
                MyString = "inner-foo"
            };

            var mockParser = new Mock<IResponseParser>();

            mockParser.Setup(p => p.SerializeMessage(complexObject))
                      .Returns("{\"json\": \"result\"}")
                      .Verifiable();

            var subject = new RestSharpRequestHandler("xxx");
            subject.ResponseParser = mockParser.Object;

            var testParams = new TestParams
            {
                MyInt = 90,
                MyComplexObject = complexObject
            };

            var result = subject.BuildRequestParams(testParams);

            Assert.Equal(2, result.Keys.Count);
            Assert.Equal("{\"json\": \"result\"}", result["my_complex_object"]);
            Assert.Equal("90", result["my_int"]);

            mockParser.Verify();
        }

        [Fact]
        public void BuildApiPathHandlesEmptyExpressions()
        {
            var subject = new RestSharpRequestHandler("xxx");

            var result = subject.BuildApiPath("/root");

            Assert.Equal("/root", result);
        }

        [Fact]
        public void BuildApiPathHandlesBasicExpressions()
        {
            var subject = new RestSharpRequestHandler("xxx");

            var result = subject.BuildApiPath("/root", SomeStringParam => "foo", test_param => true);

            Assert.Equal("/root?SomeStringParam=foo&test_param=true", result);
        }

        [Fact]
        public void BuildApiPathHandlesUriDataExpressions()
        {
            var subject = new RestSharpRequestHandler("xxx");

            var result = subject.BuildApiPath("/root", UriData => "https://url.com?x=y&#");

            Assert.Equal("/root?UriData=https%3A%2F%2Furl.com%3Fx%3Dy%26%23", result);
        }

        [Fact]
        public void ExecuteAndDeserializeRequestHandlesBasicRequest()
        {
            var token = "xxx";
            var path = "/root";
            var jsonPayload = "{\"json\": \"result\"";

            var restResponse = new RestResponse();
            restResponse.Content = jsonPayload;

            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(r => r.Execute(It.IsAny<IRestRequest>()))
                          .Callback<IRestRequest>(r => 
                          {
                              Assert.Equal(0, r.Files.Count);
                              Assert.Equal(Method.POST, r.Method);
                              Assert.Equal(path, r.Resource);
                              Assert.Equal(1, r.Parameters.Count);
                              Assert.Equal(token, r.Parameters[0].Value);
                          })
                          .Returns(restResponse)
                          .Verifiable();

            var mockParser = new Mock<IResponseParser>();
            mockParser.Setup(p => p.Deserialize<TestParams>(jsonPayload))
                      .Returns(new TestParams { MyInt = 90 })
                      .Verifiable();

            var subject = new RestSharpRequestHandler(token);
            subject.RestClient = mockRestClient.Object;
            subject.ResponseParser = mockParser.Object;

            var result = subject.ExecuteAndDeserializeRequest<TestParams>(path);

            Assert.Equal(90, result.MyInt);
            mockParser.Verify();
            mockRestClient.Verify();
        }

        [Fact]
        public void ExecuteAndDeserializeRequestHandlesComplexRequest()
        {
            var token = "xxx";
            var path = "/root";
            var jsonPayload = "{\"json\": \"result\"";

            var dictionaryParams = new Dictionary<string, string>
            {
                { "foo", "bar" }
            };

            var fileRequest = new FileUploadRequest
            {
                Title = "file",
                Filename = "file.txt",
                FileData = new byte[10]
            };

            var restResponse = new RestResponse();
            restResponse.Content = jsonPayload;

            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(r => r.Execute(It.IsAny<IRestRequest>()))
                          .Callback<IRestRequest>(r =>
                          {
                              Assert.Equal(1, r.Files.Count);
                              Assert.Equal(fileRequest.Title, r.Files[0].Name);
                              Assert.Equal(fileRequest.Filename, r.Files[0].FileName);
                              Assert.Equal(fileRequest.FileData.Length, r.Files[0].ContentLength);
                              Assert.Equal(Method.PUT, r.Method);
                              Assert.Equal(path, r.Resource);
                              Assert.Equal(2, r.Parameters.Count);
                              Assert.Equal(token, r.Parameters.First(p => p.Name == "token").Value);
                              Assert.Equal("bar", r.Parameters.First(p => p.Name == "foo").Value);
                          })
                          .Returns(restResponse)
                          .Verifiable();

            var mockParser = new Mock<IResponseParser>();
            mockParser.Setup(p => p.Deserialize<TestParams>(jsonPayload))
                      .Returns(new TestParams { MyInt = 90 })
                      .Verifiable();

            var subject = new RestSharpRequestHandler(token);
            subject.RestClient = mockRestClient.Object;
            subject.ResponseParser = mockParser.Object;

            var result = subject.ExecuteAndDeserializeRequest<TestParams>(path, dictionaryParams, HttpMethod.PUT, fileRequest);

            Assert.Equal(90, result.MyInt);
            mockParser.Verify();
            mockRestClient.Verify();
        }

        public enum TestEnumParams
        {
            TestItem1,
            TestItem2
        }

        public class TestParams
        {
            public string MyString { get; set; }
            public bool? MyBool { get; set; }
            public int MyInt { get; set; }
            public byte[] MyBytes { get; set; }
            public TestEnumParams? MyEnum { get; set; }
            public TestParams MyComplexObject { get; set; }
            private string MyPrivateString { get; set; }
        }
    }
}
