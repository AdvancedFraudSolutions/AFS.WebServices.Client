using System;
using System.Linq;
using System.Threading.Tasks;

namespace AFS.WebServices.Client.Example
{
    static class Extensions
    {
        /// <summary>
        /// Hides the complication of handling aggregate exceptions for async methods that are normally awaited.
        /// </summary>
        /// <remarks>
        /// This method is only being used to simplify this demo application.
        /// Normally, async methods will be awaited in GUI applications.
        /// Do not use this method in your application!
        /// </remarks>
        public static T GetResult<T>(this Task<T> task)
        {
            try
            {
                return task.Result;
            }
            catch (AggregateException ex)
            {
                var flat = ex.Flatten();
                if (flat.InnerExceptions.Count > 1)
                    throw;

                throw flat.InnerExceptions[0];
            }
        }

        /// <summary>
        /// Hides the complication of handling aggregate exceptions for async methods that are normally awaited.
        /// </summary>
        /// <remarks>
        /// This method is only being used to simplify this demo application.
        /// Normally, async methods will be awaited in GUI applications.
        /// Do not use this method in your application!
        /// </remarks>
        public static void WaitNicely(this Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                var flat = ex.Flatten();
                if (flat.InnerExceptions.Count > 1)
                    throw;

                throw flat.InnerExceptions[0];
            }
        }

        public static T Prompt<T>(this string prompt, Func<string, T> parser, T @default = default(T))
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            try
            {
                return string.IsNullOrEmpty(input) ? @default : parser(input);
            }
            catch
            {
                return @default;
            }
        }

        public static string Prompt(this string prompt)
        {
            return Prompt(prompt, x => x);
        }

        public static string GetEnumChoices(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("The type is not an enum: " + type.FullName);

            return string.Join(", ", Enum.GetValues(type).Cast<object>().Select(x => string.Format("{0:D} {0}", x)));
        }
    }
}