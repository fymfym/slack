using RestSharp;
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
    public class RestSharpRequestHandler : IRequestHandler
    {
        private readonly string apiKey;

        public IRestClient RestClient { get; set; } = new RestClient("https://slack.com/api");
        public IResponseParser ResponseParser { get; set; } = new ResponseParser();

        public RestSharpRequestHandler(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            this.apiKey = apiKey;
        }

        public Dictionary<string, string> BuildRequestParams<T>(T requestParamsObject)
        {
            if (requestParamsObject == null)
                return new Dictionary<string, string>();

            var requestParams = new Dictionary<string, string>();
            var publicProps = typeof(T).GetProperties();

            foreach (var paramProp in publicProps)
            {
                // TODO: maybe easier to whitelist types instead of blacklist
                if (paramProp.PropertyType.IsAssignableFrom(typeof(byte[])))
                    continue;

                var key = paramProp.Name.ToDelimitedString('_');
                object value = paramProp.GetMethod.Invoke(requestParamsObject, null);

                if (value == null)
                    continue;

                var stringValue = value.ToString();

                if (value is bool)
                    stringValue = stringValue.ToLower();
                else if (value.GetType().IsEnum)
                    stringValue = stringValue.ToDelimitedString('_');
                else if (!value.GetType().IsPrimitive && !(value is string))
                    stringValue = this.ResponseParser.SerializeMessage(value);

                requestParams.Add(key, stringValue);
            }

            return requestParams;
        }

        public string BuildApiPath(string apiPath, params Expression<Func<string, object>>[] queryParamParts)
        {
            if (queryParamParts == null)
                return apiPath;

            var queryParams = new List<string>();

            foreach (var paramPart in queryParamParts)
            {
                var key = paramPart.Parameters[0].Name;
                var value = paramPart.Compile().Invoke("");

                if (value == null)
                    continue;

                if (value is bool || value is bool?)
                    value = value.ToString().ToLower();

                queryParams.Add($"{key}={Uri.EscapeDataString(value.ToString())}");
            }

            if (queryParams.Count < 1)
                return apiPath;

            return $"{apiPath}?" + string.Join("&", queryParams);
        }

        public T ExecuteAndDeserializeRequest<T>(string apiPath, Dictionary<string, string> parameters = null, HttpMethod method = HttpMethod.POST, FileUploadRequest file = null)
        {
            var response = ExecuteRequest(apiPath, parameters, method, file);

            // TODO: handle error response

            var result = this.ResponseParser.Deserialize<T>(response.Content);

            return result;
        }

        private IRestResponse ExecuteRequest(string apiPath, Dictionary<string, string> parameters = null, HttpMethod method = HttpMethod.POST, FileUploadRequest file = null)
        {
            var request = new RestRequest(apiPath, (Method)Enum.Parse(typeof(Method), method.ToString()));
            request.AddParameter("token", this.apiKey);

            if (file != null)
                request.AddFile("file", file.FileData, file.Filename);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter.Value == null)
                        continue;

                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            return this.RestClient.Execute(request);
        }
    }
}
