using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model.Requests;

namespace Tab.Slack.WebApi
{
    public interface IRequestHandler
    {
        IResponseParser ResponseParser { get; set; }

        Dictionary<string, string> BuildRequestParams<T>(T requestParamsObject);
        string BuildApiPath(string apiPath, params Expression<Func<string, object>>[] queryParamParts);
        T ExecuteAndDeserializeRequest<T>(string apiPath, Dictionary<string, string> parameters = null, HttpMethod method = HttpMethod.POST, FileUploadRequest file = null);
    }
}
