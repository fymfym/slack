using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Tab.Slack.Common.Json
{
    // http://www.vicesoftware.com/uncategorized/extending-json-net-to-serialize-json-properties-using-a-format-that-is-delimited-by-dashes-and-all-lower-case/
    public class LowerCaseDelimitedPropertyNamesContractResolver : DefaultContractResolver
    {
        private readonly char delimiter;

        public LowerCaseDelimitedPropertyNamesContractResolver(char delimiter)
        {
            this.delimiter = delimiter;
        }
        
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToDelimitedString(this.delimiter);
        }
    }
}
