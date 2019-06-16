namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.OData.Client;

    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    using CommerceProxy.Sitecore.Commerce.Engine;
    using System.Security.Cryptography.X509Certificates;

    [PipelineDisplayName(EnvironmentProxyConstants.GetProxyContainerBlock)]
    public class GetProxyContainerBlock : PipelineBlock<GetProxyContainerArgument, Container, CommercePipelineExecutionContext>
    {
        public GetProxyContainerBlock()
          : base(null)
        {
        }

        public override Task<Container> Run(GetProxyContainerArgument arg, CommercePipelineExecutionContext context)
        {
            GetProxyContainerBlock getComposerTemplatesBlock = this;
            Condition.Requires(arg).IsNotNull(getComposerTemplatesBlock.Name + ": The GetContainerArgument cannot be null.");

            Container container = new Container(new Uri(arg.BaseUri));
            container.MergeOption = MergeOption.OverwriteChanges;
            container.DisableInstanceAnnotationMaterialization = true;
            container.BuildingRequest += (s, e) =>
            {
                e.Headers.Add("ShopName", arg.ShopName);
                e.Headers.Add("Environment", arg.EnvironmentName);
                e.Headers.Add("ShopperId", arg.ShopperId);
                e.Headers.Add("CustomerId", arg.CustomerId);
                e.Headers.Add("Language", arg.Language);
                e.Headers.Add("Currency", arg.Currency);

                DateTimeOffset dateTimeOffset = DateTime.Now;
                e.Headers.Add("EffectiveDate", dateTimeOffset.ToString(CultureInfo.InvariantCulture));

                string certificate = this.GetCertificate(arg);
                if (certificate == null)
                    return;
                e.Headers.Add(arg.CommerceEngineCertHeaderName, certificate);
            };

            return Task.FromResult(container);
        }

        private string GetCertificate(GetProxyContainerArgument arg)
        {
            X509Store x509Store = new X509Store((StoreName)arg.CertStoreName, (StoreLocation)arg.CertStoreLocation);
            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, arg.CertThumbprint, false);
            if (certificate2Collection.Count == 0)
                return null;
            X509Certificate2 x509Certificate2 = certificate2Collection[0];
            if (x509Certificate2 == null)
                return null;
            string certificate = Convert.ToBase64String(x509Certificate2.Export(X509ContentType.Cert), Base64FormattingOptions.None);
            return certificate;
        }
    }
}
