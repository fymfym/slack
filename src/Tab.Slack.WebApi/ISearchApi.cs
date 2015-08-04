using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface ISearchApi
    {
        SearchResponse All(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null);
        SearchResponse Files(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null);
        SearchResponse Messages(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null);
    }
}
