using System;

namespace AFS.WebServices.Client
{
    public static class Extensions
    {
        public static T Parse<T>(this string s, Func<string, T> parser)
        {
            return parser(s);
        }

        internal static void EnsureContentType(this ISerializeToRequestStream i, string contentType, string requiredContentType)
        {
            if (!contentType.Equals(requiredContentType, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentOutOfRangeException("contentType",
                    string.Format("contentType must be {0}", requiredContentType));
        }


        internal static void EnsureContentTypeStartsWith(this ISerializeToRequestStream i, string contentType, string requiredContentTypePrefix)
        {
            if (!contentType.StartsWith(requiredContentTypePrefix, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentOutOfRangeException("contentType",
                    string.Format("contentType must be {0}", requiredContentTypePrefix));
        }

        internal static void EnsureContentType(this IDeserializeFromResponseStream i, string contentType, string requiredContentType)
        {
            EnsureContentType(null as ISerializeToRequestStream, contentType, requiredContentType);
        }


        internal static void EnsureContentTypeStartsWith(this IDeserializeFromResponseStream i, string contentType, string requiredContentTypePrefix)
        {
            EnsureContentTypeStartsWith(null as ISerializeToRequestStream, contentType, requiredContentTypePrefix);

        }
    }
}