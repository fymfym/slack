using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface IFilesApi
    {
        ResponseBase Delete(string fileId);
        FileResponse Info(string fileId, int commentsCount = 100, int pageNumber = 1);
        FilesResponse List(FilesListRequest request);
        FileResponse Upload(FileUploadRequest request);
    }
}
