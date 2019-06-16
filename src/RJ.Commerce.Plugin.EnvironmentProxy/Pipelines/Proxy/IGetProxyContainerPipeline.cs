namespace RJ.Commerce.Plugin.EnvironmentProxy
{
    using Sitecore.Commerce.Core;   
    using Sitecore.Framework.Pipelines;

    using CommerceProxy.Sitecore.Commerce.Engine;

    [PipelineDisplayName(EnvironmentProxyConstants.GetProxyContainerPipeline)]
    public interface IGetProxyContainerPipeline : IPipeline<GetProxyContainerArgument, Container, CommercePipelineExecutionContext>, IPipelineBlock<GetProxyContainerArgument, Container, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
