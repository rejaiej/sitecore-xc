namespace RJ.Commerce.Plugin.Composer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    using EnvironmentProxy;

    [PipelineDisplayName(ComposerConstants.DoActionSelectCommerceEngineEnvironmentToPullTemplatesBlock)]
    public class DoActionSelectTemplatesToPullFromEnvironmentBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ComposerCommander _commander;
        private readonly IGetProxyComposerTemplatesPipeline _getProxyComposerTemplatesPipeline;

        public DoActionSelectTemplatesToPullFromEnvironmentBlock(ComposerCommander commander, IGetProxyComposerTemplatesPipeline getProxyComposerTemplatesPipeline)
          : base(null)
        {
            this._commander = commander;
            this._getProxyComposerTemplatesPipeline = getProxyComposerTemplatesPipeline;
        }

        public override async Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            DoActionSelectTemplatesToPullFromEnvironmentBlock thisBlock = this;
            Condition.Requires(arg).IsNotNull(thisBlock.Name + ": The entity view cannot be null.");

            if (string.IsNullOrEmpty(arg.Action) || (!arg.Action.Equals(context.GetPolicy<KnownComposerActionsPolicy>().SelectTemplatesToPullFromEnvironment, StringComparison.OrdinalIgnoreCase)))
            {
                return await Task.FromResult(arg);
            }

            KnownResultCodes errorsCodes = context.CommerceContext.GetPolicy<KnownResultCodes>();
            ViewProperty environmentProperty = arg.GetProperty(ComposerConstants.EnvironmentDropdownPropertyName);
            string propertyDisplayName = environmentProperty == null ? ComposerConstants.EnvironmentDropdownPropertyDisplayName : environmentProperty.DisplayName;
            if (string.IsNullOrEmpty(environmentProperty?.Value))
            {
                string str = await context.CommerceContext.AddMessage(errorsCodes.ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    propertyDisplayName
                }, "Invalid or missing value for property '" + propertyDisplayName + "'.").ConfigureAwait(false);
                return arg;
            }

            // get ids from view
            string[] templateIds = arg.GetProperties(p => p.UiType.Equals("Options", StringComparison.OrdinalIgnoreCase))
                .Where(property => {
                    bool result;
                    bool.TryParse(property.Value, out result);
                    return result;
                })
                .Select(property => property.Name)
                .ToArray();

            if (templateIds == null || templateIds.Length <= 0) return arg;

            var composerTemplates = (await thisBlock._getProxyComposerTemplatesPipeline.Run(new GetProxyComposerTemplatesArgument(templateIds, environmentProperty.Value), context).ConfigureAwait(false))
                .ToList();
            if (composerTemplates == null || !composerTemplates.Any())
            {
                string str = await context.CommerceContext.AddMessage(errorsCodes.ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    propertyDisplayName
                }, "Invalid or missing value for property '" + propertyDisplayName + "'.").ConfigureAwait(false);
                return arg;
            }

            foreach(var template in composerTemplates)
            {
                await this._commander.CreateTemplateFromProxy(context.CommerceContext, template);
            }

            return arg;
        }
    }
}
