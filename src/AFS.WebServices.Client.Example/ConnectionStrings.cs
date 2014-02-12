using System.Configuration;
using System.Runtime.CompilerServices;

namespace AFS.WebServices.Client.Example
{
    public static class ConnectionStrings
    {
        public static string AFSApi { get { return GetConnectionString(); } }

        public static string GetConnectionString([CallerMemberName]string name = null)
        {
            var settings = ConfigurationManager.ConnectionStrings[name];
            return settings == null ? null : settings.ConnectionString;
        }
    }
}