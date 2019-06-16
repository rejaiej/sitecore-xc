namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;

    public class GetProxyContainerArgument : PipelineArgument
    {
        public GetProxyContainerArgument(
            string baseUri,
            string shopName,
            string environmentName,
            string shopperId,
            string customerId,
            string language,
            string currency,
            string commerceEngineCertHeaderName,
            string certThumbprint,
            int certStoreLocation,
            int certStoreName
            )
        {
            this.BaseUri = baseUri;
            this.ShopName = shopName;
            this.EnvironmentName = environmentName;
            this.ShopperId = shopperId;
            this.CustomerId = customerId;
            this.Language = language;
            this.Currency = currency;
            this.CommerceEngineCertHeaderName = commerceEngineCertHeaderName;
            this.CertThumbprint = certThumbprint;
            this.CertStoreLocation = certStoreLocation;
            this.CertStoreName = certStoreName;
        }

        public string BaseUri { get; set; } = string.Empty;

        public string ShopName { get; set; } = "CommerceEngineDefaultStorefront";

        public string EnvironmentName { get; set; } = "HabitatAuthoring";

        public string ShopperId { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public string Language { get; set; } = "en-US";

        public string Currency { get; set; } = "USD";

        public string CommerceEngineCertHeaderName { get; set; } = "X-CommerceEngineCert";

        public string CertThumbprint { get; set; } = string.Empty;

        public int CertStoreLocation { get; set; } = 2;

        public int CertStoreName { get; set; } = 5;

    }
}
