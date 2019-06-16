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

    [PipelineDisplayName(ComposerConstants.GetSelectCommerceEngineEnvironmentToPullTemplatesBlock)]
    public class GetSelectCommerceEngineEnvironmentToPullTemplatesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public GetSelectCommerceEngineEnvironmentToPullTemplatesBlock()
          : base(null)
        {
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            GetSelectCommerceEngineEnvironmentToPullTemplatesBlock getSelectCommerceEngineEnvironmentToPullTemplatesBlock = this;
            Condition.Requires(arg).IsNotNull(getSelectCommerceEngineEnvironmentToPullTemplatesBlock.Name + ": The entity view cannot be null.");

            if (string.IsNullOrEmpty(arg.Action) || (!arg.Action.Equals(context.GetPolicy<KnownComposerActionsPolicy>().SelectCommerceEngineEnvironmentToPullTemplates, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.FromResult(arg);
            }

            var ceEnvironmentPolicy = context.GetPolicy<KnownApplicationEnvironmentsPolicy>();
            if(ceEnvironmentPolicy == null || ceEnvironmentPolicy.Environments == null || !ceEnvironmentPolicy.Environments.Any())
            {
                return Task.FromResult(arg);
            }

            AvailableSelectionsPolicy availableEnvironments = new AvailableSelectionsPolicy();
            ceEnvironmentPolicy.Environments.ForEach(e =>
            {
                List<Selection> list = availableEnvironments.List;
                list.Add(new Selection()
                {
                    Name = e.Name,
                    DisplayName = e.Name
                });
            });

            List<ViewProperty> properties = arg.Properties;
            ViewProperty viewProperty = new ViewProperty(new List<Policy>()
            {
                availableEnvironments
            });

            viewProperty.Name = ComposerConstants.EnvironmentDropdownPropertyName;
            viewProperty.RawValue = string.Empty;
            properties.Add(viewProperty);
            return Task.FromResult(arg);
        }
    }
}
