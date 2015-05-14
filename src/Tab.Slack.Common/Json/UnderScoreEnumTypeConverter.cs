using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Common.Json
{
    public class UnderscoreEnumTypeConverter : StringEnumConverter
    {
        public static string FindMatchingName<T>(string name)
        {
            return FindMatchingName(name, typeof(T));
        }

        public static string FindMatchingName(string name, Type enumType)
        {
            var searchName = (name ?? "").Replace("_", "");

            var enumValues = Enum.GetNames(enumType);
            return enumValues.FirstOrDefault(value => value.Equals(searchName, StringComparison.CurrentCultureIgnoreCase));
        }

        public override object ReadJson(JsonReader reader, Type enumType, object existingValue, JsonSerializer serializer)
        {
            var jsonValue = ((string)reader.Value);
            var enumValue = FindMatchingName(jsonValue, enumType);

            if (enumValue == null)
                return null;

            return Enum.Parse(enumType, enumValue, false);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
    }
}
