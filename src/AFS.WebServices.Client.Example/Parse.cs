using System;
using System.Linq;

namespace AFS.WebServices.Client.Example
{
    static class Parse
    {
        public static bool Bool(string s)
        {
            return new[] { "y", "t", "1" }.Any(x => s.StartsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        public static T Enum<T>(string value)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("The type is not an enum: " + type.FullName);

            return (T)System.Enum.Parse(type, value, true);

        }
    }
}