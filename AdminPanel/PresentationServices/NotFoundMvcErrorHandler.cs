using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.PresentationServices;

public class NotFoundMvcErrorHandler : IMvcErrorHandler
{
    public ValueTask Handle(MvcErrorHandleContext context)
    {
        if (context.Error is KeyNotFoundException or HttpRequestException { StatusCode: HttpStatusCode.NotFound })
            context.Complete(new NotFoundResult());
        return default;
    }
}