using System.Security.Authentication;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.App.Exceptions;

public class CommonExceptionsDescriber : IExceptionsDescriber
{
    protected readonly ILogger<CommonExceptionsDescriber> Logger;

    public CommonExceptionsDescriber(ILogger<CommonExceptionsDescriber> logger) => Logger = logger;

    public virtual ActionResult Describe(object exception)
    {
        switch (exception)
        {
            case KeyNotFoundException notFound:
                return new NotFoundObjectResult(notFound.Message);
            case NotPermittedException notPermitted:
                return new ObjectResult(new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = nameof(NotPermittedException),
                    Detail = notPermitted.Message
                });
            case InvalidCredentialException cred:
                return new UnauthorizedObjectResult(cred.Message);
            case ActionFailedException actionFailed:
                return new ConflictObjectResult(actionFailed.Message);
            case UnsuitableDataException unsuitableData:
                return new BadRequestObjectResult(unsuitableData.ToProblemDetails());
            case ArgumentException arg:
                return new BadRequestObjectResult(arg.Message);
            default:
                if (exception is Exception ex)
                    Logger.LogWarning(ex, $"Exception couldn't be described by {typeof(CommonExceptionsDescriber)}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}