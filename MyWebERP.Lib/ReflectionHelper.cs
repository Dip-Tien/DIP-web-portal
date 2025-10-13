using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace MyWebERP.Lib
{
    public static class ReflectionHelper
    {
        private static readonly Dictionary<(Type, string), PropertyInfo?> _propCache = new();

        public static object? GetValue(object? item, string propertyName)
        {
            if (item == null || string.IsNullOrWhiteSpace(propertyName))
                return null;

            if (item is IDictionary<string, object> dict)
                return dict.TryGetValue(propertyName, out var val) ? val : null;

            var type = item.GetType();
            var key = (type, propertyName);

            if (!_propCache.TryGetValue(key, out var prop))
            {
                prop = type.GetProperty(propertyName);
                _propCache[key] = prop;
            }

            return prop?.GetValue(item);
        }

        public static DateTime? GetDate(object? item, string propertyName)
        {
            var val = GetValue(item, propertyName);
            if (val is DateTime dt) return dt;
            if (DateTime.TryParse(val?.ToString(), out var parsed)) return parsed;
            return null;
        }
    }
}
