namespace RJ.Commerce.Plugin.Composer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Composer;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName(ComposerConstants.GetComposerTemplatesBlock)]
    public class GetComposerTemplatesBlock : PipelineBlock<GetComposerTemplatesArgument, IEnumerable<ComposerTemplate>, CommercePipelineExecutionContext>
    {
        private readonly IFindEntitiesInListPipeline _findEntitiesInListPipeline;

        public GetComposerTemplatesBlock(IFindEntitiesInListPipeline findEntitiesInListPipeline)
          : base((string)null)
        {
            this._findEntitiesInListPipeline = findEntitiesInListPipeline;
        }

        public override async Task<IEnumerable<ComposerTemplate>> Run(GetComposerTemplatesArgument arg, CommercePipelineExecutionContext context)
        {
            GetComposerTemplatesBlock getComposerTemplatesBlock = this;
            Condition.Requires(arg).IsNotNull(getComposerTemplatesBlock.Name + ": The entity view cannot be null.");

            IEnumerable<ComposerTemplate> composerTemplates = Enumerable.Empty<ComposerTemplate>();

            if (arg.TemplateIds == null || arg.TemplateIds.Length <= 0)
            {
                return composerTemplates;
            }

            composerTemplates = (await getComposerTemplatesBlock._findEntitiesInListPipeline.Run(new FindEntitiesInListArgument(typeof(ComposerTemplate), CommerceEntity.ListName<ComposerTemplate>() ?? "", 0, int.MaxValue), context).ConfigureAwait(false))
                ?.List
                ?.Items
                .Cast<ComposerTemplate>()
                .Where(x => arg.TemplateIds.Contains(x.Id))
                .ToList();

            return composerTemplates;
        }
    }
}
