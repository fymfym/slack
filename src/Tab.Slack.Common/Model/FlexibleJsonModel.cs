using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tab.Slack.Common.Model
{
    public abstract class FlexibleJsonModel
    {
        [JsonExtensionData]
        public JObject UnmatchedAdditionalJsonData { get; set; }

        public IEnumerable<string> WalkUnmatchedData(string path = null)
        {
            if (path == null)
                path = this.GetType().Name;

            var unmatchedDataEntries = new List<string>();

            if (this.UnmatchedAdditionalJsonData != null && this.UnmatchedAdditionalJsonData.HasValues)
                unmatchedDataEntries.Add($"Unmatched Model Data <{path}>: {this.UnmatchedAdditionalJsonData.ToString()}");

            foreach (var prop in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.PropertyType.IsGenericType &&
                    typeof(FlexibleJsonModel).IsAssignableFrom(prop.PropertyType.GetGenericArguments()[0]) &&
                    typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                    )
                {
                    var values = prop.GetValue(this) as IEnumerable;

                    if (values != null)
                    {
                        foreach (var item in values)
                        {
                            var unmatchedElementEntries = (item as FlexibleJsonModel).WalkUnmatchedData($"{path}.{prop.Name}[]");

                            if (unmatchedElementEntries.Any())
                                unmatchedDataEntries.AddRange(unmatchedElementEntries);
                        }
                    }
                }
                else if (typeof(FlexibleJsonModel).IsAssignableFrom(prop.PropertyType))
                {
                    var value = prop.GetValue(this);
                    var unmatchedPropertyEntries = (value as FlexibleJsonModel).WalkUnmatchedData($"{path}.{prop.Name}");

                    if (unmatchedPropertyEntries.Any())
                        unmatchedDataEntries.AddRange(unmatchedPropertyEntries);
                }
            }

            return unmatchedDataEntries;
        }
    }
}
