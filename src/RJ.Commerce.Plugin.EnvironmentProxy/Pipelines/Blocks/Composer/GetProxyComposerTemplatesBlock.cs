namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.ServiceProxy;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    using Extensions;

    [PipelineDisplayName(EnvironmentProxyConstants.GetProxyComposerTemplatesBlock)]
    public class GetProxyComposerTemplatesBlock : PipelineBlock<GetProxyComposerTemplatesArgument, IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate>, CommercePipelineExecutionContext>
    {
        private readonly IGetProxyContainerPipeline _getProxyContainerPipeline;

        public GetProxyComposerTemplatesBlock(IGetProxyContainerPipeline getProxyContainerPipeline)
          : base(null)
        {
            this._getProxyContainerPipeline = getProxyContainerPipeline;
        }

        public override async Task<IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate>> Run(GetProxyComposerTemplatesArgument arg, CommercePipelineExecutionContext context)
        {
            GetProxyComposerTemplatesBlock getProxyComposerTemplatesBlock = this;
            Condition.Requires(arg).IsNotNull(getProxyComposerTemplatesBlock.Name + ": The GetContainerArgument cannot be null.");

            var appEnvironment = context.GetPolicy<KnownApplicationEnvironmentsPolicy>()?.Environments
                .FirstOrDefault(x => x.Name.Equals(arg.ApplicationEnvironmentName, System.StringComparison.InvariantCultureIgnoreCase));
            var getProxyContainerArgument = new GetProxyContainerArgument(
                appEnvironment.BaseUri,
                appEnvironment.ShopName,
                appEnvironment.Environment,
                appEnvironment.ShopperId,
                appEnvironment.CustomerId,
                appEnvironment.Language,
                appEnvironment.Currency,
                appEnvironment.CommerceEngineCertHeaderName,
                appEnvironment.CertThumbprint,
                appEnvironment.CertStoreLocation,
                appEnvironment.CertStoreName);

            var container = await getProxyComposerTemplatesBlock._getProxyContainerPipeline.Run(getProxyContainerArgument, context).ConfigureAwait(false);
            IEnumerable<CommerceProxy.Sitecore.Commerce.Plugin.Composer.ComposerTemplate> composerTemplates;
            if (arg.TemplateIds != null && arg.TemplateIds.Length > 0)
            {
                composerTemplates = Proxy.Execute(container.ComposerTemplatesByIdsExpandComponents(arg.TemplateIds));
            }
            else
            {
                composerTemplates = Proxy.Execute(container.ComposerTemplates.Expand("Components"));
            }

            return composerTemplates;
        }
    }
}
