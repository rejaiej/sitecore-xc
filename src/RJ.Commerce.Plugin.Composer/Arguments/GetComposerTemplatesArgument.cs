namespace RJ.Commerce.Plugin.Composer
{
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;

    public class GetComposerTemplatesArgument : PipelineArgument
    {
        public GetComposerTemplatesArgument(string[] templateIds)
        {
            this.TemplateIds = templateIds;
        }

        public GetComposerTemplatesArgument()
        {
            this.TemplateIds = new List<string>()
                .ToArray();
        }

        public string[] TemplateIds { get; set; }

    }
}
