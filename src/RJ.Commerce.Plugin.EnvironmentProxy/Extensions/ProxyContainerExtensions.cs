namespace RJ.Commerce.Plugin.EnvironmentProxy.Extensions
{
    using System.Collections.Generic;

    using Microsoft.OData.Client;

    using CommerceProxy.Sitecore.Commerce.Engine;
    using CommerceProxy.Sitecore.Commerce.Plugin.Composer;

    public static class ProxyContainerExtensions
    {
        // HACK: Could not find a way to expand entities in an action
        public static DataServiceActionQuery<ComposerTemplate> ComposerTemplatesByIdsExpandComponents(this Container container, ICollection<string> rawIds)
        {
            return new DataServiceActionQuery<ComposerTemplate>(container, container.BaseUri.OriginalString.Trim('/') + "/ComposerTemplatesByIds?$expand=Components", new BodyOperationParameter("rawIds", rawIds));
        }
    }
}
