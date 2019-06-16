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
    public class DoActionSelectCommerceEngineEnvironmentToPullTemplatesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly IGetProxyComposerTemplatesPipeline _getProxyComposerTemplatesPipeline;

        public DoActionSelectCommerceEngineEnvironmentToPullTemplatesBlock(IGetProxyComposerTemplatesPipeline getProxyComposerTemplatesPipeline)
          : base(null)
        {
            this._getProxyComposerTemplatesPipeline = getProxyComposerTemplatesPipeline;
        }

        public override async Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            DoActionSelectCommerceEngineEnvironmentToPullTemplatesBlock thisBlock = this;
            Condition.Requires(arg).IsNotNull(thisBlock.Name + ": The entity view cannot be null.");

            if (string.IsNullOrEmpty(arg.Action) || (!arg.Action.Equals(context.GetPolicy<KnownComposerActionsPolicy>().SelectCommerceEngineEnvironmentToPullTemplates, StringComparison.OrdinalIgnoreCase)))
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


            var composerTemplates = (await thisBlock._getProxyComposerTemplatesPipeline.Run(new GetProxyComposerTemplatesArgument(null, environmentProperty.Value), context).ConfigureAwait(false))
                .ToList();
            if (composerTemplates == null || !composerTemplates.Any())
            {
                string str = await context.CommerceContext.AddMessage(errorsCodes.ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    propertyDisplayName
                }, "Invalid or missing value for property '" + propertyDisplayName + "'.").ConfigureAwait(false);
                return arg;
            }

            environmentProperty.RawValue = environmentProperty.Value;
            environmentProperty.IsReadOnly = true;
            ViewProperty newEnvironmentProperty = environmentProperty;

            List<Policy> policyList = new List<Policy>();
            List<Selection> selectionList = new List<Selection>();
            Selection selection = new Selection();

            selection.DisplayName = environmentProperty.Value;
            selection.Name = environmentProperty.Value;
            selectionList.Add(selection);
            policyList.Add(new AvailableSelectionsPolicy(selectionList, false));
            newEnvironmentProperty.Policies = policyList;
            composerTemplates.ForEach(template =>
            {
                List<ViewProperty> properties = arg.Properties;
                properties.Add(new ViewProperty()
                {
                    DisplayName = $"{template.DisplayName} ({template.Id})",
                    Name = template.Id,
                    RawValue = false,
                    UiType = "Options"
                });
            });
            context.CommerceContext.AddModel(new MultiStepActionModel(context.GetPolicy<KnownComposerActionsPolicy>().SelectTemplatesToPullFromEnvironment));
            return arg;
        }
    }
}
