namespace AFS.WebServices.Client.TrueChecks
{

    /// <summary>
    /// Constructs urls for client interaction with service
    /// </summary>
    public static class Urls
    {
        public const string DefaultBaseAddress = "https://api.advancedfraudsolutions.com";
        private const string TrueChecksPrefix = UrlFragments.TrueChecksPrefix + "/";
        internal const string TrueChecksSearch = TrueChecksPrefix + UrlFragments.TrueChecksSearch;
        internal const string TrueChecksClientSettings = TrueChecksPrefix + UrlFragments.TrueChecksClientSettings;
        internal const string TrueChecksCheckAction = TrueChecksPrefix + UrlFragments.TrueChecksCheckAction;
        internal const string TrueChecksRoutingLookup = TrueChecksPrefix + UrlFragments.RoutingLookup;

        internal static string TrueChecksImages(string imagePath)
        {
            return string.Format("{0}{1}/{2}", TrueChecksPrefix, UrlFragments.TrueChecksImages, imagePath);
        }
    }
}
