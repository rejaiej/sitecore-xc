namespace RJ.Commerce.Plugin.Composer.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http.OData;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Composer;

    [EnableQuery]
    [Route("api")]
    public class ComposerTemplatesController : Sitecore.Commerce.Plugin.Composer.ComposerTemplatesController
    {
        public ComposerTemplatesController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPost]
        [Route("ComposerTemplatesByIds")]
        public virtual async Task<IActionResult> ComposerTemplatesByIds([FromBody] ODataActionParameters parameters)
        {
            ComposerTemplatesController composerTemplatesController = this;
            if (!composerTemplatesController.ModelState.IsValid)
            {
                return new BadRequestObjectResult(Enumerable.Empty<ComposerTemplate>());
            }

            if (!parameters.ContainsKey("rawIds") || !(parameters["rawIds"] is JArray))
            {
                return new BadRequestObjectResult(Enumerable.Empty<ComposerTemplate>());
            }

            var templateIds = (parameters["rawIds"] as JArray)?.ToObject<string[]>();
            var composerTemplates = await composerTemplatesController.Command<GetComposerTemplatesCommand>().Process(composerTemplatesController.CurrentContext, templateIds).ConfigureAwait(false);
            return new ObjectResult(composerTemplates);
        }
    }
}
