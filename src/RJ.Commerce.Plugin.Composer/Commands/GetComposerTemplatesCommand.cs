namespace RJ.Commerce.Plugin.Composer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Commerce.Plugin.Composer;

    public class GetComposerTemplatesCommand : CommerceCommand
    {
        private readonly IGetComposerTemplatesPipeline _getComposerTemplatesPipeline;

        public GetComposerTemplatesCommand(
            IGetComposerTemplatesPipeline getComposerTemplatesPipeline,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this._getComposerTemplatesPipeline = getComposerTemplatesPipeline;
        }

        public virtual async Task<IEnumerable<ComposerTemplate>> Process(CommerceContext commerceContext, string[] templateIds)
        {
            GetComposerTemplatesCommand getComposerTemplatesCommand = this;

            IEnumerable<ComposerTemplate> composerTemplates = Enumerable.Empty<ComposerTemplate>();
            GetComposerTemplatesArgument getComposerTemplatesArgument = new GetComposerTemplatesArgument(templateIds);

            using (CommandActivity.Start(commerceContext, getComposerTemplatesCommand))
            {
                await getComposerTemplatesCommand.PerformTransaction(commerceContext, async () =>
                {
                    CommercePipelineExecutionContextOptions pipelineContextOptions = commerceContext.PipelineContextOptions;
                    composerTemplates = await this._getComposerTemplatesPipeline.Run(getComposerTemplatesArgument, pipelineContextOptions).ConfigureAwait(false);
                }).ConfigureAwait(false);

                return composerTemplates;
            }
        }
    }
}
