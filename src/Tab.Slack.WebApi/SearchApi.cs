using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class SearchApi : ISearchApi
    {
        private readonly IRequestHandler request;

        public SearchApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public SearchResponse All(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null)
        {
            string highlighted = null;

            if (isHighlighted.HasValue)
                highlighted = isHighlighted.Value ? "1" : "0";

            var apiPath = this.request.BuildApiPath("/search.all",
                                        query => queryString,
                                        sort => sortType,
                                        sort_dir => sortDir,
                                        highlight => highlighted,
                                        count => messageCount,
                                        page => pageNumber);

            var response = this.request.ExecuteAndDeserializeRequest<SearchResponse>(apiPath);

            return response;
        }

        public SearchResponse Files(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null)
        {
            string highlighted = null;

            if (isHighlighted.HasValue)
                highlighted = isHighlighted.Value ? "1" : "0";

            var apiPath = this.request.BuildApiPath("/search.files",
                                        query => queryString,
                                        sort => sortType,
                                        sort_dir => sortDir,
                                        highlight => highlighted,
                                        count => messageCount,
                                        page => pageNumber);

            var response = this.request.ExecuteAndDeserializeRequest<SearchResponse>(apiPath);

            return response;
        }

        public SearchResponse Messages(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null)
        {
            string highlighted = null;

            if (isHighlighted.HasValue)
                highlighted = isHighlighted.Value ? "1" : "0";

            var apiPath = this.request.BuildApiPath("/search.messages",
                                        query => queryString,
                                        sort => sortType,
                                        sort_dir => sortDir,
                                        highlight => highlighted,
                                        count => messageCount,
                                        page => pageNumber);

            var response = this.request.ExecuteAndDeserializeRequest<SearchResponse>(apiPath);

            return response;
        }
    }
}
