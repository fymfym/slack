using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class FilesApi : IFilesApi
    {
        private readonly IRequestHandler request;

        public FilesApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public ResponseBase Delete(string fileId)
        {
            var apiPath = this.request.BuildApiPath("/files.delete", file => fileId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public FileResponse Info(string fileId, int commentsCount = 100, int pageNumber = 1)
        {
            var apiPath = this.request.BuildApiPath("/files.info",
                                       file => fileId,
                                       count => commentsCount.ToString(),
                                       page => pageNumber.ToString()
                                      );
            var response = this.request.ExecuteAndDeserializeRequest<FileResponse>(apiPath);

            return response;
        }

        public FilesResponse List(FilesListRequest request)
        {
            var requestParams = this.request.BuildRequestParams(request);
            var response = this.request.ExecuteAndDeserializeRequest<FilesResponse>("/files.list", requestParams);

            return response;
        }

        public FileResponse Upload(FileUploadRequest request)
        {
            var requestParams = this.request.BuildRequestParams(request);
            var response = this.request.ExecuteAndDeserializeRequest<FileResponse>("/files.upload", requestParams, file: request);

            return response;
        }
    }
}
