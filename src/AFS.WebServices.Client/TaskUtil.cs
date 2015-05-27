using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AFS.WebServices.Client
{
    internal static class TaskUtil
    {
        /// <summary>
        /// Convenience method so that I can avoid the #if compiler directives.
        /// </summary>
        public static Task<T> ReadAsAsync<T>(HttpContent content, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (content == null) throw new ArgumentNullException("content");

#if NET40
            return content.ReadAsAsync<T>();
#else
            return content.ReadAsAsync<T>(cancellationToken);
#endif

        }

        public static Task AsCompletedTask(this object obj)
        {
#if NET40
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(obj);
            return tcs.Task;
#else
            return Task.FromResult(obj);
#endif
        }

    }
}