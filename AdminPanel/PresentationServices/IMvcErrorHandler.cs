namespace AdminPanel.PresentationServices;

public interface IMvcErrorHandler
{
    public ValueTask Handle(MvcErrorHandleContext handleContext);
}