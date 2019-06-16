namespace RJ.Commerce.Plugin.Composer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Composer;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    

    [PipelineDisplayName(ComposerConstants.PopulateComposerTemplatesViewActionsBlock)]
    public class PopulateComposerTemplatesViewActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ListCommander _commander;

        public PopulateComposerTemplatesViewActionsBlock(ListCommander commander)
          : base((string)null)
        {
            this._commander = commander;
        }

        public override async Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            PopulateComposerTemplatesViewActionsBlock viewActionsBlock = this;
            Condition.Requires(arg).IsNotNull(viewActionsBlock.Name + ": The argument cannot be null");

            EntityViewArgument entityViewArgument = context.CommerceContext.GetObjects<EntityViewArgument>().FirstOrDefault();
            KnownComposerViewsPolicy composerViews = context.GetPolicy<KnownComposerViewsPolicy>();
            if (!string.IsNullOrEmpty(arg.Name) && (arg.Name.Equals(composerViews.ComposerTemplates, StringComparison.OrdinalIgnoreCase) || entityViewArgument?.Entity is ComposerTemplate && arg.Name.Equals(composerViews.Details, StringComparison.OrdinalIgnoreCase)))
            {
                KnownComposerActionsPolicy actions = context.GetPolicy<KnownComposerActionsPolicy>();
                ActionsPolicy actionsPolicy = arg.GetPolicy<ActionsPolicy>();

                List<string> stringList;
                if (arg.Name.Equals(composerViews.ComposerTemplates, StringComparison.OrdinalIgnoreCase))
                {
                    stringList = await viewActionsBlock._commander.GetListItemIds<ComposerTemplate>(context.CommerceContext, CommerceEntity.ListName<ComposerTemplate>(), 0, 1).ConfigureAwait(false);
                }

                else
                {
                    stringList = new List<string>();
                }

                List<string> source = stringList;
                bool flag = !arg.Name.Equals(composerViews.ComposerTemplates, StringComparison.OrdinalIgnoreCase) || source != null && source.Any<string>();

                List<Policy> policyList = new List<Policy>();
                MultiStepActionPolicy stepActionPolicy = new MultiStepActionPolicy();
                EntityActionView entityActionView1 = new EntityActionView();
                entityActionView1.Name = actions.SelectCommerceEngineEnvironmentToPullTemplates;
                entityActionView1.IsEnabled = flag;
                entityActionView1.EntityView = actions.SelectTemplatesToPullFromEnvironment;
                stepActionPolicy.FirstStep = entityActionView1;
                policyList.Add(stepActionPolicy);

                List<Policy> commercePolicies = policyList;
                List<EntityActionView> actions1 = actionsPolicy.Actions;
                EntityActionView entityActionView2 = new EntityActionView(commercePolicies);
                entityActionView2.Name = actions.PullTemplatesFromEnvironment;
                entityActionView2.IsEnabled = flag;
                entityActionView2.EntityView = string.Empty;
                entityActionView2.Icon = "add";
                actions1.Add(entityActionView2);

                actions = null;
                actionsPolicy = null;
            }
            return arg;
        }
    }
}
