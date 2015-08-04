using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class FileApiTests : ApiTestBase
    {
        [Fact]
        public void FileDeleteShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/files.delete?file=foo");

            var subject = new FilesApi(requestHandlerMock.Object);
            var result = subject.Delete("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void FileInfoShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<FileResponse>("/files.info?file=foo&count=30&page=1");

            var subject = new FilesApi(requestHandlerMock.Object);
            var result = subject.Info("foo", 30);

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void FileListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<FilesResponse>("/files.list");

            var request = new FilesListRequest { Types = "all" };

            requestHandlerMock.Setup(r => r.BuildRequestParams(request))
                              .Returns<Dictionary<string, string>>(null)
                              .Verifiable();

            var subject = new FilesApi(requestHandlerMock.Object);
            var result = subject.List(request);

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void FileUploadShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<FileResponse>("/files.upload");

            var request = new FileUploadRequest
            {
                Filename = "hello.txt",
                FileData = Encoding.ASCII.GetBytes("hello world")
            };

            requestHandlerMock.Setup(r => r.BuildRequestParams(request))
                              .Returns<Dictionary<string, string>>(null)
                              .Verifiable();

            var subject = new FilesApi(requestHandlerMock.Object);
            var result = subject.Upload(request);

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
