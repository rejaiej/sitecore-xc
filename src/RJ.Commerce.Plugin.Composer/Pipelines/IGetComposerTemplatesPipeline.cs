namespace RJ.Commerce.Plugin.Composer
{
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;   
    using Sitecore.Commerce.Plugin.Composer;
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName(ComposerConstants.GetComposerTemplatesPipeline)]
    public interface IGetComposerTemplatesPipeline : IPipeline<GetComposerTemplatesArgument, IEnumerable<ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock<GetComposerTemplatesArgument, IEnumerable<ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
