namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;   
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName(EnvironmentProxyConstants.GetProxyComposerTemplatesPipeline)]
    public interface IGetProxyComposerTemplatesPipeline : IPipeline<GetProxyComposerTemplatesArgument, IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock<GetProxyComposerTemplatesArgument, IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
