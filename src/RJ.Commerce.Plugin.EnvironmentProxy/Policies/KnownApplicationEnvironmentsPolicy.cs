namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;

    public class KnownApplicationEnvironmentsPolicy : Policy
    {
        public KnownApplicationEnvironmentsPolicy()
        {
            this.Environments = new List<CommerceEngineApplicationEnvironmentModel>();
        }

        public IList<CommerceEngineApplicationEnvironmentModel> Environments { get; set; }
    }
}