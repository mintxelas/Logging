// based on: https://andrewlock.net/using-anonymous-types-and-tuples-to-attach-correlation-ids-to-scope-state-with-serilog-and-seq-in-asp-net-core/

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Logging
{
    public static class Extensions
    {
        public static IDisposable BeginScopeWith(this ILogger logger, object values)
        {
            var dictionary = GetValuesAsDictionary(values);
            return logger.BeginScope(dictionary);
        }

        private static IEnumerable<KeyValuePair<string, object>> GetValuesAsDictionary(object values)
        {
            var valuesAsDictionary = values as IEnumerable<KeyValuePair<string, object>>;
            if (valuesAsDictionary != null)
            {
                return valuesAsDictionary;
            }
            var valuesDictionary = new Dictionary<string, object>();
            if (values != null)
            {
                foreach (PropertyHelper property in PropertyHelper.GetProperties(values.GetType()))
                {
                    // Extract the property values from the property helper
                    // The advantage here is that the property helper caches fast accessors.
                    valuesDictionary.Add(property.Name, property.GetValue(values));
                }
            }
            return valuesDictionary;
        }
    }
}
