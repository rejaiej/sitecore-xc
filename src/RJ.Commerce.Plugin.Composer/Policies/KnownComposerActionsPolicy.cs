namespace RJ.Commerce.Plugin.Composer
{
    public class KnownComposerActionsPolicy : global::Sitecore.Commerce.Plugin.Composer.KnownComposerActionsPolicy
    {
        public KnownComposerActionsPolicy()
        {
            this.PullTemplatesFromEnvironment = nameof(this.PullTemplatesFromEnvironment);
            this.SelectCommerceEngineEnvironmentToPullTemplates = nameof(this.SelectCommerceEngineEnvironmentToPullTemplates);
            this.SelectTemplatesToPullFromEnvironment = nameof(this.SelectTemplatesToPullFromEnvironment);
        }

        public string PullTemplatesFromEnvironment { get; set; }

        public string SelectCommerceEngineEnvironmentToPullTemplates { get; set; }

        public string SelectTemplatesToPullFromEnvironment { get; set; }
    }
}