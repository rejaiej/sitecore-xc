namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    public static class EnvironmentProxyConstants
    {
        public static class Prefixes
        {
            public const string RJCommerceBlock = "RJ.Commerce.EnvironmentProxy.block.";

            public const string RJCommercePipeline = "RJ.Commerce.EnvironmentProxy.pipeline.";
        }

        public const string GetProxyComposerTemplatesBlock = Prefixes.RJCommerceBlock + "GetProxyComposerTemplatesPipeline";

        public const string GetProxyComposerTemplatesPipeline = Prefixes.RJCommercePipeline + "GetProxyComposerTemplatesPipeline";

        public const string GetProxyContainerBlock = Prefixes.RJCommerceBlock + "GetProxyContainerBlock";

        public const string GetProxyContainerPipeline = Prefixes.RJCommercePipeline + "GetProxyContainerPipeline";

        public const string RegisteredPluginBlock = Prefixes.RJCommerceBlock + "RegisteredPlugin";
    }
}