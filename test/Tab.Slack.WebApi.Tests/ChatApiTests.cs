using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class ChatApiTests : ApiTestBase
    {
        [Fact]
        public void ChatDeleteShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChatDeleteResponse>("/chat.delete?channel=foo&ts=1111.2222");

            var subject = new ChatApi(requestHandlerMock.Object);
            var result = subject.Delete("foo", "1111.2222");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
        
        [Fact]
        public void ChatPostMessageShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<MessageResponse>("/chat.postMessage", new MessageResponse { Ok = true });
            var responseParserMock = new Mock<IResponseParser>();

            responseParserMock.Setup(r => r.RemapMessageToConcreteType(It.IsAny<MessageBase>()))
                              .Returns(new MessageBase())
                              .Verifiable();

            requestHandlerMock.Setup(r => r.ResponseParser)
                              .Returns(responseParserMock.Object)
                              .Verifiable();
            
            var request = new PostMessageRequest
            {
                Channel = "foo",
                Parse = ParseMode.Full,
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Text = "Attach1",
                        Fields = new List<Field>
                        {
                            new Field { Title = "F1", Value = "V1" },
                            new Field { Title = "F2", Value = "V2" },
                        }
                    }
                }
            };

            requestHandlerMock.Setup(r => r.BuildRequestParams(request))
                              .Returns<Dictionary<string, string>>(null)
                              .Verifiable();

            var subject = new ChatApi(requestHandlerMock.Object);
            var result = subject.PostMessage(request);

            requestHandlerMock.Verify();
            responseParserMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChatUpdateShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChatUpdateResponse>("/chat.update?channel=foo&ts=1111.2222&text=bar");

            var subject = new ChatApi(requestHandlerMock.Object);
            var result = subject.Update("foo", "1111.2222", "bar");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
