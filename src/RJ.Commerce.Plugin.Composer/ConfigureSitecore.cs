namespace RJ.Commerce.Plugin.Composer
{
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Pipelines(
                config =>
                    config
                        .ConfigurePipeline<IConfigureServiceApiPipeline>(c =>
                        {
                            c.Add<ConfigureServiceApiBlock>();
                        }, "main", 1000)
                        .ConfigurePipeline<IDoActionPipeline>(c =>
                        {
                            c.Add<DoActionSelectCommerceEngineEnvironmentToPullTemplatesBlock>().After<ValidateEntityVersionBlock>(null, Matcher.Each);
                            c.Add<DoActionSelectTemplatesToPullFromEnvironmentBlock>().After<ValidateEntityVersionBlock>(null, Matcher.Each);
                        }, "main", 1000)
                        .AddPipeline<IGetComposerTemplatesPipeline, GetComposerTemplatesPipeline>(c =>
                        {
                            c.Add<GetComposerTemplatesBlock>();
                        }, "main", 1000)
                        .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                        {
                            c.Add<GetSelectCommerceEngineEnvironmentToPullTemplatesBlock>().After<PopulateEntityVersionBlock>(null, Matcher.Each);
                        }, "main", 1000)
                        .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                        {
                            c.Add<PopulateComposerTemplatesViewActionsBlock>().After<Sitecore.Commerce.Plugin.Composer.PopulateComposerTemplatesViewActionsBlock>(null, Matcher.Each);
                        }, "main", 1000)
                        .ConfigurePipeline<IRunningPluginsPipeline>(c =>
                        {
                            c.Add<RegisteredPluginBlock>().After<RunningPluginsBlock>();
                        }, "main", 1000)
                );
        }
    }
}