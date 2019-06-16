namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System;

    using Sitecore.Commerce.Core;

    public class CommerceEngineApplicationEnvironmentModel : Model
    {
        private readonly string _environmentId;

        public CommerceEngineApplicationEnvironmentModel()
        {
            this._environmentId = Guid.NewGuid().ToString("N");
        }

        public string EnvironmentId {
            get
            {
                return _environmentId;
            }
        }

        public string BaseUri { get; set; } = string.Empty;

        public string ShopName { get; set; } = "CommerceEngineDefaultStorefront";

        public string Environment { get; set; } = "HabitatAuthoring";

        public string ShopperId { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public string Language { get; set; } = "en-US";

        public string Currency { get; set; } = "USD";

        public string CommerceEngineCertHeaderName { get; set; } = "X-CommerceEngineCert";

        public string CertThumbprint { get; set; }

        public int CertStoreLocation { get; set; } = 2;

        public int CertStoreName { get; set; } = 5;
    }
}
