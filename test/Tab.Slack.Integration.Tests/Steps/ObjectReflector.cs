using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tab.Slack.Integration.Tests.Steps
{
    public static class ObjectReflector
    {
        public static T GetObjectPathValue<T>(object target, string complexFieldPath)
        {
            var values = GetObjectPathValues(target, complexFieldPath);

            if (values == null)
                return default(T);

            return (T)values.Cast<object>().FirstOrDefault();
        }

        public static IEnumerable GetObjectPathValues(object target, string complexFieldPath)
        {
            var resultValues = new List<object>();
            IEnumerable targetObjects;

            if (target is IEnumerable && !(target is string))
            {
                targetObjects = (IEnumerable)target;
            }
            else
            {
                targetObjects = new ArrayList { target };
            }

            if (string.IsNullOrWhiteSpace(complexFieldPath))
            {
                return targetObjects;
            }

            var fieldLookups = complexFieldPath.Split(new[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
            complexFieldPath = string.Join(" > ", fieldLookups.Skip(1).Select(l => l.Trim()));

            var match = Regex.Match(fieldLookups[0], @"(?<field>[^\[ ]+)(\[(?<selector>[^\]]+)\])?");
            var field = match.Groups["field"].Value;
            var selector = match.Groups["selector"].Value;

            foreach (var targetObject in targetObjects)
            {
                var fieldValue = GetPropertyValue(targetObject, field);

                if (!string.IsNullOrWhiteSpace(selector) && selector != "*")
                    fieldValue = LookupObjectValueViaIndexer(fieldValue, selector, complexFieldPath);

                var enumerable = fieldValue as IEnumerable;
                if (enumerable != null && !(fieldValue is string))
                {
                    resultValues.AddRange(enumerable.Cast<object>());
                }
                else if (fieldValue != null)
                {
                    resultValues.Add(fieldValue);
                }
            }

            return resultValues;
        }

        private static object LookupObjectValueViaIndexer(object targetObject, string selector, string complexFieldPath)
        {    
            var targetEnumerable = targetObject as IEnumerable;

            if (targetEnumerable == null)
                throw new InvalidOperationException("You can not use a selector to index a non-enumerable object");

            var targetDictionary = targetObject as IDictionary;

            if (targetDictionary != null)
            {
                var item = targetDictionary[selector];

                var itemResults = GetObjectPathValues(item, complexFieldPath);
                return itemResults;
            }

            var list = targetEnumerable.Cast<object>();

            int indexSelector;
            if (int.TryParse(selector, out indexSelector))
            {
                var item = list.Skip(indexSelector).First();

                var itemResults = GetObjectPathValues(item, complexFieldPath);
                return itemResults;
            }

            return null;
        }

        public static object GetPropertyValue(object targetObject, string propertyName)
        {
            var targetProperty = targetObject.GetType().GetProperty(propertyName);

            if (targetProperty == null)
            {
                // attempt a fuzzy match
                var fuzzyName = propertyName.Replace("*", ".*");
                var allProperties = targetObject.GetType().GetProperties();

                targetProperty = allProperties.FirstOrDefault(p => Regex.IsMatch(p.Name, fuzzyName, RegexOptions.IgnoreCase));
            }

            if (targetProperty == null)
                throw new Exception("Failed to find property: " + propertyName);

            return targetProperty.GetValue(targetObject);
        }

        public static PropertyInfo FindProperty(Type targetObject, params string[] searchNames)
        {
            var allProperties = targetObject.GetProperties();

            foreach (var searchName in searchNames)
            {
                var targetProperty = allProperties.FirstOrDefault(p => p.Name == searchName);

                if (targetProperty != null)
                    return targetProperty;
            }

            return null;
        }

        public static object ExecuteMethod(object targetObject, string methodName, Dictionary<string, string> methodArguments = null)
        {
            var targetMethod = targetObject.GetType().GetMethod(methodName);
            methodArguments = methodArguments ?? new Dictionary<string, string>();

            if (targetMethod == null)
            {
                // attempt a fuzzy match
                var fuzzyName = methodName.Replace("*", ".*");
                var allMethods = targetObject.GetType().GetMethods();

                // TODO: doesn't currently support overloaded methods
                targetMethod = allMethods.FirstOrDefault(p => Regex.IsMatch(p.Name, fuzzyName, RegexOptions.IgnoreCase));
            }

            if (targetMethod == null)
                throw new Exception("Failed to find method: " + methodName);

            var args = new List<object>();

            foreach (var parameterInfo in targetMethod.GetParameters())
            {
                object parameter = null;

                if (parameterInfo.HasDefaultValue)
                    parameter = parameterInfo.DefaultValue;

                if (methodArguments.ContainsKey(parameterInfo.Name.ToLower()))
                {
                    var parameterValue = methodArguments[parameterInfo.Name.ToLower()];

                    if (parameterInfo.ParameterType.IsArray)
                    {
                        var elementType = parameterInfo.ParameterType.GetElementType();
                        var elementValues = parameterValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(p => p.Trim())
                                                          .Select(p => Convert.ChangeType(p, elementType))
                                                          .ToArray();

                        var parameterArray = Array.CreateInstance(elementType, elementValues.Length);
                        elementValues.CopyTo(parameterArray, 0);

                        parameter = parameterArray;
                    }
                    else
                    {
                        parameter = Convert.ChangeType(parameterValue, parameterInfo.ParameterType);
                    }
                }

                // TODO: support complex type parameters

                args.Add(parameter);
            }

            return targetMethod.Invoke(targetObject, args.ToArray());
        }
    }
}
