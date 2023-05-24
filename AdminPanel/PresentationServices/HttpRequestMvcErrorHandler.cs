namespace AdminPanel.PresentationServices;

public class HttpRequestMvcErrorHandler : IMvcErrorHandler
{
    public int OrderFactor => int.MaxValue;

    public ValueTask Handle(MvcErrorHandleContext context)
    {
        if (context.Error is not HttpRequestException { StatusCode: not null } exception) return default;

        context.ViewModel.StatusCode = exception.StatusCode!.Value;
        context.ViewModel.Message = exception.Message;
        return default;
    }
}