using Common.Domain.Utils;

namespace AdminPanel.PresentationServices;

public interface IMvcErrorHandler : IHasOrderFactor
{
    public ValueTask Handle(MvcErrorHandleContext handleContext);

    int IHasOrderFactor.OrderFactor => 0;
}