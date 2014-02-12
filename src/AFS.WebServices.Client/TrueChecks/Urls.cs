namespace AFS.WebServices.Client.TrueChecks
{

    /// <summary>
    /// Constructs urls for client interaction with service
    /// </summary>
    internal static class Urls
    {
        public const string DefaultBaseAddress = "https://api.advancedfraudsolutions.com";
        private const string TrueChecksPrefix = UrlFragments.TrueChecksPrefix + "/";
        public const string TrueChecksSearch = TrueChecksPrefix + UrlFragments.TrueChecksSearch;
        public const string TrueChecksClientSettings = TrueChecksPrefix + UrlFragments.TrueChecksClientSettings;
        public const string TrueChecksCheckAction = TrueChecksPrefix + UrlFragments.TrueChecksCheckAction;

        public static string TrueChecksImages(string imagePath)
        {
            return string.Format("{0}{1}/{2}", TrueChecksPrefix, UrlFragments.TrueChecksImages, imagePath);
        }
    }
}
