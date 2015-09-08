using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi.Tests
{
    public abstract class ApiTestBase
    {
        public Mock<IRequestHandler> ExecRequestMock<T>(string endpoint, T instance = null) where T : class, new()
        {
            var requestHandlerMock = new Mock<IRequestHandler>();

            requestHandlerMock.Setup(r => r.ExecuteAndDeserializeRequest<T>(
                                              It.IsAny<string>(),
                                              It.IsAny<Dictionary<string, string>>(),
                                              It.IsAny<HttpMethod>(),
                                              It.IsAny<FileUploadRequest>()))
                              .Callback<string, Dictionary<string, string>, HttpMethod, FileUploadRequest>((e,_1,_2,_3) => CheckEndpointCall(endpoint, e))
                              .Returns(instance ?? new T())
                              .Verifiable();

            return requestHandlerMock;
        }

        public Mock<IRequestHandler> PathAndExecRequestMock<T>(string endpoint) where T : ResponseBase, new()
        {
            var requestHandlerMock = ExecRequestMock<T>(endpoint);

            requestHandlerMock.Setup(r => r.BuildApiPath(It.IsAny<string>(), It.IsAny<Expression<Func<string, object>>[]>()))
                              .Returns(endpoint)
                              .Verifiable();

            return requestHandlerMock;
        }

        private void CheckEndpointCall(string expectedEndpoint, string actualEndpoint)
        {
            if (expectedEndpoint != actualEndpoint)
                throw new ArgumentException($"Mock verification failed; provided endpoint ({actualEndpoint}) does not match expected endpoint ({expectedEndpoint})");
        }
    }
}
