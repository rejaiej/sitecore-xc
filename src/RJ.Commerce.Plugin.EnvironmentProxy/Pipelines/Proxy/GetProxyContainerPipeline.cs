namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using Microsoft.Extensions.Logging;

    using Sitecore.Commerce.Core;   
    using Sitecore.Framework.Pipelines;

    using CommerceProxy.Sitecore.Commerce.Engine;

    public class GetProxyContainerPipeline : CommercePipeline<GetProxyContainerArgument, Container>, IGetProxyContainerPipeline, IPipeline<GetProxyContainerArgument, Container, CommercePipelineExecutionContext>, IPipelineBlock<GetProxyContainerArgument, Container, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public GetProxyContainerPipeline(
            IPipelineConfiguration<IGetProxyContainerPipeline> configuration,
            ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}
