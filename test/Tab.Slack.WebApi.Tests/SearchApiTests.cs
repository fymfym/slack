using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class SearchApiTests : ApiTestBase
    {
        [Fact]
        public void SearchAllShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<SearchResponse>("/search.all?query=foo");

            var subject = new SearchApi(requestHandlerMock.Object);
            var result = subject.All(queryString: "foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void SearchFilesShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<SearchResponse>("/search.files?query=foo");

            var subject = new SearchApi(requestHandlerMock.Object);
            var result = subject.Files(queryString: "foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void SearchMessagesShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<SearchResponse>("/search.messages?query=foo");

            var subject = new SearchApi(requestHandlerMock.Object);
            var result = subject.Messages(queryString: "foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
