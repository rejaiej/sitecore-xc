namespace RJ.Commerce.Plugin.Composer
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Sitecore.Commerce.Core;   
    using Sitecore.Commerce.Plugin.Composer;
    using Sitecore.Framework.Pipelines;

    public class GetComposerTemplatesPipeline : CommercePipeline<GetComposerTemplatesArgument, IEnumerable<ComposerTemplate>>, IGetComposerTemplatesPipeline, IPipeline<GetComposerTemplatesArgument, IEnumerable<ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock<GetComposerTemplatesArgument, IEnumerable<ComposerTemplate>, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public GetComposerTemplatesPipeline(
            IPipelineConfiguration<IGetComposerTemplatesPipeline> configuration,
            ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}
