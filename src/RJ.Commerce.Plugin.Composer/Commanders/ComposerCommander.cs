namespace RJ.Commerce.Plugin.Composer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Commerce.Plugin.Composer;
    using Sitecore.Commerce.Plugin.ManagedLists;
    using Sitecore.Commerce.Plugin.Views;
    using Sitecore.Framework.Conditions;

    using ProxyCore = CommerceProxy.Sitecore.Commerce.Core;
    using ProxyCatalog = CommerceProxy.Sitecore.Commerce.Plugin.Catalog;
    using ProxyComposer = CommerceProxy.Sitecore.Commerce.Plugin.Composer;
    using ProxyEntityViews = CommerceProxy.Sitecore.Commerce.EntityViews;
    using ProxyManagedLists = CommerceProxy.Sitecore.Commerce.Plugin.ManagedLists;
    using ProxyViews = CommerceProxy.Sitecore.Commerce.Plugin.Views;

    public class ComposerCommander : Sitecore.Commerce.Plugin.Composer.ComposerCommander
    {
        public ComposerCommander(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task CreateTemplateFromProxy(CommerceContext commerceContext, ProxyComposer.ComposerTemplate proxyComposerTemplate)
        {
            ComposerCommander composerCommander = this;
            Condition.Requires(commerceContext).IsNotNull("CommerceContext");
            Condition.Requires(proxyComposerTemplate).IsNotNull("composerTemplate");
            using (CommandActivity.Start(commerceContext, composerCommander))
            {
                KnownResultCodes errorCodes = commerceContext.GetPolicy<KnownResultCodes>();
                string composerTemplateName = proxyComposerTemplate.Name;
                if (string.IsNullOrEmpty(composerTemplateName))
                {
                    string composerTemplatePropertyName = "Name";
                    string str2 = await commerceContext.AddMessage(errorCodes.ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                    {
                        composerTemplatePropertyName
                    }, "Invalid or missing value for property '" + composerTemplatePropertyName + "'.").ConfigureAwait(false);
                    return;
                }
                string templateId = proxyComposerTemplate.Id;
                if (await composerCommander.GetEntity<ComposerTemplate>(commerceContext, templateId, false).ConfigureAwait(false) != null)
                {
                    string str = await commerceContext.AddMessage(errorCodes.ValidationError, "NameAlreadyInUse", new object[1]
                    {
                        proxyComposerTemplate.Name
                    }, "Name '" + proxyComposerTemplate.Name + "' is already in use.").ConfigureAwait(false);
                    return;
                }

                ComposerTemplate engineComposerTemplate = new ComposerTemplate(templateId);
                engineComposerTemplate.Name = proxyComposerTemplate.Name;
                engineComposerTemplate.DisplayName = proxyComposerTemplate.DisplayName;
                engineComposerTemplate.FriendlyId = proxyComposerTemplate.Name;
                engineComposerTemplate.LinkedEntities = proxyComposerTemplate.LinkedEntities;

                this.AddTagsFromProxy(engineComposerTemplate, proxyComposerTemplate);
                this.AddComposerTemplateListMembershipsFromProxy(engineComposerTemplate, proxyComposerTemplate);
                await this.AddComposerTemplateEntityViewFromProxy(commerceContext, engineComposerTemplate, proxyComposerTemplate);
                this.AddItemDefinitionsFromProxy(engineComposerTemplate, proxyComposerTemplate);

                int num = await composerCommander.PersistEntity(commerceContext, engineComposerTemplate).ConfigureAwait(false) ? 1 : 0;
                errorCodes = null;
                templateId = null;
            }
        }

        private void AddComposerTemplateListMembershipsFromProxy(ComposerTemplate engineComposerTemplate, ProxyComposer.ComposerTemplate proxyComposerTemplate)
        {
            var proxyListMembershipComponent = proxyComposerTemplate?.Components?.OfType<ProxyManagedLists.ListMembershipsComponent>().FirstOrDefault();
            if (proxyListMembershipComponent == null) return;

            foreach(var membership in proxyListMembershipComponent.Memberships)
            {
                engineComposerTemplate.GetComponent<ListMembershipsComponent>().Memberships.Add(membership);
            }
        }

        private async Task AddComposerTemplateEntityViewFromProxy(CommerceContext commerceContext, ComposerTemplate engineComposerTemplate, ProxyComposer.ComposerTemplate proxyComposerTemplate)
        {
            var proxyEntityViewComponent = proxyComposerTemplate.Components.OfType<ProxyViews.EntityViewComponent>().FirstOrDefault();
            if (proxyEntityViewComponent == null ||
                proxyEntityViewComponent.View == null ||
                proxyEntityViewComponent.View.ChildViews == null ||
                !proxyEntityViewComponent.View.ChildViews.Any())
            {
                return;
            }

            var proxyChildView = proxyEntityViewComponent.View?.ChildViews[0] as ProxyEntityViews.EntityView;
            EntityView engineEntityChildView = new EntityView();
            engineEntityChildView.ItemId = proxyChildView.ItemId;
            engineEntityChildView.EntityId = proxyChildView.EntityId;
            engineEntityChildView.Name = proxyChildView?.Name;
            engineEntityChildView.DisplayName = proxyChildView.DisplayName;
            engineEntityChildView.Icon = proxyChildView.Icon;
            engineEntityChildView.DisplayRank = proxyChildView.DisplayRank;

            var isNameValid = await this.RunValidCheckOnName(engineEntityChildView, commerceContext);
            if (!isNameValid)
            {
                return;
            }

            engineComposerTemplate.GetComponent<EntityViewComponent>().AddChildView(engineEntityChildView);
            if(proxyChildView.Properties != null)
            {
                foreach(var proxyViewProperty in proxyChildView.Properties)
                {
                    await this.AddPropertyToViewFromProxy(commerceContext, engineEntityChildView, proxyViewProperty);
                }
            }
        }

        private void AddItemDefinitionsFromProxy(ComposerTemplate engineComposerTemplate, ProxyComposer.ComposerTemplate proxyComposerTemplate)
        {
            var proxyItemDefinitionsComponent = proxyComposerTemplate?.Components?.OfType<ProxyCatalog.ItemDefinitionsComponent>().FirstOrDefault();
            if (proxyItemDefinitionsComponent == null) return;

            engineComposerTemplate.GetComponent<ItemDefinitionsComponent>().AddDefinitions(proxyItemDefinitionsComponent.Definitions);
        }

        private async Task AddPropertyToViewFromProxy(CommerceContext commerceContext, EntityView engineEntityView, ProxyEntityViews.ViewProperty proxyViewProperty)
        {

            ViewProperty engineViewProperty = new ViewProperty();
            engineViewProperty.Name = proxyViewProperty.Name;
            engineViewProperty.DisplayName = proxyViewProperty.DisplayName;
            engineViewProperty.OriginalType = proxyViewProperty.OriginalType;
            engineViewProperty.IsHidden = proxyViewProperty.IsHidden;
            engineViewProperty.IsReadOnly = proxyViewProperty.IsReadOnly;
            engineViewProperty.IsRequired = proxyViewProperty.IsRequired;
            engineViewProperty.UiType = proxyViewProperty.UiType;
            engineViewProperty.RawValue = proxyViewProperty.Value;

            var isNameValid = await this.RunValidCheckOnName(engineViewProperty, commerceContext);
            if (!isNameValid)
            {
                return;
            }

            var proxyAvailableSelectionsPolicy = proxyViewProperty.Policies.OfType<ProxyCore.AvailableSelectionsPolicy>().FirstOrDefault();
            if(proxyAvailableSelectionsPolicy != null && engineViewProperty.IsText())
            {
                this.AddOptionConstraintToPropertyFromProxy(engineViewProperty, proxyAvailableSelectionsPolicy);
            }

            var proxyMinMaxPolicy = proxyViewProperty.Policies.OfType<ProxyCore.MinMaxValuePolicy>().FirstOrDefault();
            if (proxyMinMaxPolicy != null && engineViewProperty.IsNumeric())
            {
                this.AddOptionConstraintToPropertyFromProxy(engineViewProperty, proxyAvailableSelectionsPolicy);
            }

            engineEntityView.Properties.Add(engineViewProperty);
        }

        private void AddOptionConstraintToPropertyFromProxy(ViewProperty engineViewProperty, ProxyCore.AvailableSelectionsPolicy proxyAvailableSelectionsPolicy)
        {
            if (proxyAvailableSelectionsPolicy.List == null || !proxyAvailableSelectionsPolicy.List.Any())
            {
                return;
            }

            AvailableSelectionsPolicy engineAvailableSelectionPolicy = engineViewProperty.GetPolicy<AvailableSelectionsPolicy>();
            foreach (var proxySelection in proxyAvailableSelectionsPolicy.List)
            {
                Selection selection = new Selection();
                selection.DisplayName = proxySelection.DisplayName;
                selection.Name = proxySelection.Name;
                selection.IsDefault = proxySelection.IsDefault;
                engineAvailableSelectionPolicy.List.Add(selection);
            }
        }

        private void AddMinMaxConstraintToPropertyFromProxy(ViewProperty engineViewProperty, ProxyCore.MinMaxValuePolicy proxyMinMaxValuePolicy)
        {
            if (!engineViewProperty.IsNumeric() || proxyMinMaxValuePolicy == null)
            {
                return;
            }

            engineViewProperty.SetPolicy(new MinMaxValuePolicy()
            {
                MinAllow = proxyMinMaxValuePolicy.MinAllow,
                MaxAllow = proxyMinMaxValuePolicy.MaxAllow
            });
        }

        private async Task<bool> RunValidCheckOnName(object target, CommerceContext commerceContext)
        {
            ValidationPolicy validationPolicy = new ValidationPolicy();
            List<Model> models = validationPolicy.Models;
            ValidationAttributes validationAttributes = new ValidationAttributes();
            validationAttributes.Name = "Name";
            validationAttributes.MaxLength = 50;
            validationAttributes.RegexValidator = "^[\\w\\s]*$";
            validationAttributes.RegexValidatorErrorCode = "AlphanumericOnly_NameValidationError";
            models.Add(validationAttributes);

            return await validationPolicy.ValidateModels(target, commerceContext.PipelineContext).ConfigureAwait(false);
        }

        private void AddTagsFromProxy(ComposerTemplate engineComposerTemplate, ProxyComposer.ComposerTemplate proxyComposerTemplate)
        {
            if (proxyComposerTemplate.Tags == null || !proxyComposerTemplate.Tags.Any()) return;

            engineComposerTemplate.Tags = proxyComposerTemplate.Tags
               .Select(pt => new Tag(pt.Name)
                {
                    Excluded = false
                })
               .ToList();
        }
    }
}
