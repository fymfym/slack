using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Json
{
    public static class StringExtensions
    {
        public static string ToDelimitedString(this string @string, char delimiter)
        {
            var camelCaseString = @string.ToCamelCaseString();
            return new string(InsertDelimiterBeforeCaps(camelCaseString.ToCharArray(), delimiter).ToArray());
        }

        public static string ToCamelCaseString(this string @string)
        {
            if (string.IsNullOrEmpty(@string) || !char.IsUpper(@string[0]))
            {
                return @string;
            }

            string lowerCasedFirstChar = char.ToLower(@string[0], CultureInfo.InvariantCulture).ToString();

            if (@string.Length > 1)
            {
                lowerCasedFirstChar = lowerCasedFirstChar + @string.Substring(1);
            }

            return lowerCasedFirstChar;
        }

        private static IEnumerable<char> InsertDelimiterBeforeCaps(IEnumerable<char> input, char delimiter)
        {
            bool lastCharWasUppper = false;

            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    if (!lastCharWasUppper)
                    {
                        yield return delimiter;
                        lastCharWasUppper = true;
                    }
                    yield return char.ToLower(c);
                    continue;
                }

                yield return c;
                lastCharWasUppper = false;
            }
        }
    }
}
