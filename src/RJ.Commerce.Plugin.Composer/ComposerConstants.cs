namespace RJ.Commerce.Plugin.Composer
{
    public static class ComposerConstants
    {
        public static class Prefixes
        {
            public const string RJCommercePipelineBlock = "RJ.Commerce.Composer.block.";

            public const string RJCommercePipeline = "RJ.Commerce.Composer.pipeline.";
        }

        public const string AddComposerTemplatesBlock = Prefixes.RJCommercePipelineBlock + "AddComposerTemplatesBlock";

        public const string ConfigureServiceApiBlock = Prefixes.RJCommercePipelineBlock + "ConfigureServiceApiBlock";

        public const string DoActionSelectCommerceEngineEnvironmentToPullTemplatesBlock = Prefixes.RJCommercePipelineBlock + "DoActionSelectCommerceEngineEnvironmentToPullTemplatesBlock";

        public const string EnvironmentDropdownPropertyName = "CommerceEngineEnvironments";

        public const string EnvironmentDropdownPropertyDisplayName = "Commerce Engine Environments";

        public const string GetComposerTemplatesBlock = Prefixes.RJCommercePipelineBlock + "GetComposerTemplatesBlock";

        public const string GetComposerTemplatesPipeline = Prefixes.RJCommercePipeline + "GetComposerTemplatesPipeline";

        public const string GetSelectCommerceEngineEnvironmentToPullTemplatesBlock = Prefixes.RJCommercePipelineBlock + "GetSelectCommerceEngineEnvironmentToPullTemplatesBlock";

        public const string PopulateComposerTemplatesViewActionsBlock = Prefixes.RJCommercePipelineBlock + "PopulateComposerTemplatesViewActionsBlock";

        public const string RegisteredPluginBlock = Prefixes.RJCommercePipelineBlock + "RegisteredPlugin";
    }
}