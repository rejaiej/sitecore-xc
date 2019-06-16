namespace RJ.Commerce.Plugin.Composer
{
    using Sitecore.Commerce.Core;

    public class CommerceEngineApplicationEnvironmentModel : Model
    {
        public string BaseUri { get; set; } = string.Empty;

        public string ShopName { get; set; } = "CommerceEngineDefaultStorefront";

        public string Environment { get; set; } = "HabitatAuthoring";

        public string ShopperId { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public string Language { get; set; } = "en-US";

        public string Currency { get; set; } = "USD";

        public string CommerceEngineCertHeaderName { get; set; } = "X-CommerceEngineCert";

        public int CertStoreLocation { get; set; } = 2;

        public int CertStoreName { get; set; } = 5;
    }
}
