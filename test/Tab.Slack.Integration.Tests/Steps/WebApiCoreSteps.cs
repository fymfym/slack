using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.WebApi;
using TechTalk.SpecFlow;
using Xunit;

namespace Tab.Slack.Integration.Tests.Steps
{
    // TODO: there is a fair amount of clean up required here
    [Binding]
    public class WebApiCoreSteps
    {
        [Given(@"I am logged in to the Slack Web API as (.+)")]
        public void GivenIAmLoggedInToTheSlackWebAPIAs(string username)
        {
            var accessKey = ConfigurationManager.AppSettings[$"webapi.{username.ToLower()}"];
            var webApi = SlackApi.Create(accessKey);

            ScenarioContext.Current["WebApi"] = webApi;
        }

        [Given(@"I am not logged in to the Slack Web API")]
        public void GivenIAmNotLoggedInToTheSlackWebAPI()
        {
            var webApi = SlackApi.Create("invalid-key");

            ScenarioContext.Current["WebApi"] = webApi;
        }

        [When(@"I call ([^ ]+)\.([^ ]+)")]
        public void WhenICall(string apiContext, string apiMethod)
        {
            var webApi = ScenarioContext.Current.Get<SlackApi>("WebApi");
            var apiContextInstance = ObjectReflector.GetPropertyValue(webApi, apiContext);

            CallWebApi(apiContextInstance, apiMethod, null);
        }

        [When(@"I call ([^ ]+)\.([^ ]+) with (.+) equal to (.+)")]
        public void WhenICallWith(string apiContext, string apiMethod, string argumentName, string value)
        {
            var webApi = ScenarioContext.Current.Get<SlackApi>("WebApi");
            var apiContextInstance = ObjectReflector.GetPropertyValue(webApi, apiContext);

            var methodArguments = new Dictionary<string, string>();
            methodArguments[argumentName] = ParseIfFormatValue(value);

            CallWebApi(apiContextInstance, apiMethod, methodArguments);
        }

        [When(@"I call ([^ ]+)\.([^ ]+) with:")]
        public void WhenICallWith(string apiContext, string apiMethod, Table table)
        {
            var webApi = ScenarioContext.Current.Get<SlackApi>("WebApi");
            var apiContextInstance = ObjectReflector.GetPropertyValue(webApi, apiContext);

            var methodArguments = new Dictionary<string, string>();
            
            foreach (var row in table.Rows)
            {
                methodArguments[row["Argument"]] = ParseIfFormatValue(row["Value"]);
            }

            CallWebApi(apiContextInstance, apiMethod, methodArguments);    
        }
        
        [Then(@"I should receive an ok response")]
        public void ThenIShouldReceiveAnOkResponse()
        {
            object response = ScenarioContext.Current["response"];

            Assert.NotNull(response);
            Assert.True(ObjectReflector.GetObjectPathValue<bool>(response, "Ok"));
        }

        [Then(@"I should receive an? (ok)? ?response matching:")]
        public void ThenIShouldReceiveAnOkResponseMatching(string checkOk, Table table)
        {
            object response = ScenarioContext.Current["response"];

            Assert.NotNull(response);

            if (checkOk == "ok")
            {
                Assert.True(ObjectReflector.GetObjectPathValue<bool>(response, "Ok"));
                Assert.Null(ObjectReflector.GetObjectPathValue<string>(response, "Error"));
            }

            foreach (var row in table.Rows)
            {
                var fieldValue = ObjectReflector.GetObjectPathValue<object>(response, row["ResponsePath"])?.ToString();

                Assert.NotNull(fieldValue);

                if (row.Keys.Contains("RegEx") && !string.IsNullOrWhiteSpace(row["RegEx"]))
                {
                    Assert.True(Regex.IsMatch(fieldValue.ToString(), row["RegEx"]));
                }
                else
                {
                    Assert.Equal(ParseIfFormatValue(row["Value"]), fieldValue.ToString());
                }
            }
        }

        [Then(@"I should receive a response with (.+) equal to (.+)")]
        public void ThenIShouldReceiveAResponseEqualTo(string complexFieldPath, string value)
        {
            object response = ScenarioContext.Current["response"];
            Assert.NotNull(response);

            value = ParseIfFormatValue(value);
            var fieldValue = ObjectReflector.GetObjectPathValue<object>(response, complexFieldPath)?.ToString();

            Assert.NotNull(fieldValue);
            Assert.Equal(value, fieldValue);
        }

        [Then(@"I should receive a response with (.+) containing (.+)")]
        public void ThenIShouldReceiveAResponseContaining(string complexFieldPath, string value)
        {
            object response = ScenarioContext.Current["response"];
            Assert.NotNull(response);

            value = ParseIfFormatValue(value);
            var fieldValue = ObjectReflector.GetObjectPathValue<object>(response, complexFieldPath)?.ToString();

            Assert.NotNull(fieldValue);
            Assert.True(fieldValue is IEnumerable);
            var match = (fieldValue as IEnumerable).Cast<object>().FirstOrDefault(e => e?.ToString() == value);

            Assert.NotNull(match);
        }

        private void CallWebApi(object apiContextInstance, string apiMethod, Dictionary<string, string> methodArguments)
        {
            // these are "live" integration tests intended to be run once overnight
            // so try not to get rate limited
            Thread.Sleep(2000);
            var result = ObjectReflector.ExecuteMethod(apiContextInstance, apiMethod, methodArguments);
            ScenarioContext.Current["response"] = result;

            if (result is FlexibleJsonModel)
            {
                Assert.Empty((result as FlexibleJsonModel).WalkUnmatchedData());
            }
        }

        private string ParseIfFormatValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            var valueLookup = ScenarioContext.Current["values"] as Dictionary<string, string>;

            if (valueLookup == null)
            {
                valueLookup = new Dictionary<string, string>();
                ScenarioContext.Current["values"] = valueLookup;
            }

            /*
                Allows name three formats:
                1. "$labelname" - e.g. a standard variable such as $tsnow
                2. "%dynamicname%" - this will be transformed into a random unique name
                3. "fixedname" - a known constant name

                All three of which can be appended with an optional field reference:
                "name:field" - will return the value of "field" on the domain object named "name"
            */
            var match = Regex.Match(value, @"(\%(?<dynamicname>[a-zA-Z\-_0-9]+)\%|(?<name>\$?[a-zA-Z\-_0-9]+))(\:(?<field>[a-zA-Z\-_0-9]+))?");

            if (match.Success)
            {
                var dynamicName = match.Groups["dynamicname"].Value;
                var name = match.Groups["name"].Value;
                var field = match.Groups["field"].Value;

                if (string.IsNullOrWhiteSpace(name))
                    name = ParseDynamicName(dynamicName);

                object domainValue = LookupDomainValueByName(name);

                if (string.IsNullOrWhiteSpace(field))
                    return domainValue?.ToString() ?? value;

                return ObjectReflector.GetPropertyValue(domainValue, field)?.ToString();

                var rand = new Random();
                var formatValue = new StringBuilder();

                foreach (var formatChar in format)
                {
                    switch (formatChar)
                    {
                        case '-':
                            formatValue.Append('-'); break;
                        case 'N':
                            formatValue.Append(rand.Next(0, 9)); break;
                        case 'S':
                            formatValue.Append((char)rand.Next(65, 90)); break;
                    }
                }

                valueLookup[name] = name + formatValue.ToString();

                return valueLookup[name];
            }
            
            return value;
        }

        private object LookupDomainValueByName(string name)
        {
            throw new NotImplementedException();
        }

        private string ParseDynamicName(string dynamicName)
        {
            throw new NotImplementedException();
        }
    }
}
