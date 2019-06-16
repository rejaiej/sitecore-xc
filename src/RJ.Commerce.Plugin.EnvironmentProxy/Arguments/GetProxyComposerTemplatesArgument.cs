namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;

    public class GetProxyComposerTemplatesArgument : PipelineArgument
    {
        public GetProxyComposerTemplatesArgument(
            string[] templatesIds,
            string applicationEnvironmentName
            )
        {
            this.TemplateIds = templatesIds;
            this.ApplicationEnvironmentName = applicationEnvironmentName;
        }

        public string[] TemplateIds { get; set; }

        public string ApplicationEnvironmentName { get; set; }
    }
}
