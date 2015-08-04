using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    // an interface for an interface representing an interface
    // this shit just got real!
    public interface IApiApi
    {
        ApiTestResponse Test(string error = null, params string[] args);
    }
}
