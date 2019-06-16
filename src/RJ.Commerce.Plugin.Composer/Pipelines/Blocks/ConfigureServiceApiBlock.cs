namespace RJ.Commerce.Plugin.Composer
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.OData.Builder;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Composer;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName(ComposerConstants.ConfigureServiceApiBlock)]
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        public override Task<ODataConventionModelBuilder> Run(
          ODataConventionModelBuilder modelBuilder,
          CommercePipelineExecutionContext context)
        {
            Condition.Requires(modelBuilder).IsNotNull(this.Name + ": The argument cannot be null.");

            ActionConfiguration composerTemplatesByIdsActions = modelBuilder.Action("ComposerTemplatesByIds");
            composerTemplatesByIdsActions.CollectionParameter<string>("rawIds");
            composerTemplatesByIdsActions.ReturnsCollectionFromEntitySet<ComposerTemplate>("ComposerTemplates");

            return Task.FromResult(modelBuilder);
        }

        public ConfigureServiceApiBlock()
          : base(null)
        {
        }
    }
}
