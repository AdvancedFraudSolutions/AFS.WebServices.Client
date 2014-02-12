using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;

namespace AFS.WebServices.Client.Example
{
    public static class AppSettings
    {
        public static DirectoryInfo ImageDirectoryPath
        {
            get { return new DirectoryInfo(GetSetting() ?? @".\Images"); }
        }

        private static string GetSetting([CallerMemberName]string name = null)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
