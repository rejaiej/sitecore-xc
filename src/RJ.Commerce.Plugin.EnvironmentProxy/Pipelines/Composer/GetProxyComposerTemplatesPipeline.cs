namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Sitecore.Commerce.Core;   
    using Sitecore.Framework.Pipelines;

    public class GetProxyComposerTemplatesPipeline : CommercePipeline<GetProxyComposerTemplatesArgument, IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate>>, IGetProxyComposerTemplatesPipeline, IPipeline<GetProxyComposerTemplatesArgument, IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock<GetProxyComposerTemplatesArgument, IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public GetProxyComposerTemplatesPipeline(
            IPipelineConfiguration<IGetProxyComposerTemplatesPipeline> configuration,
            ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}
