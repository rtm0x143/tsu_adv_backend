using Common.Domain.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Services;

public class OrderingAuthorizationHandlerProvider : IAuthorizationHandlerProvider
{
    private Task<IEnumerable<IAuthorizationHandler>> _handlersTask;
    
    public OrderingAuthorizationHandlerProvider(IEnumerable<IAuthorizationHandler> handlers)
    {
        _handlersTask = Task.FromResult(
            handlers.Cast<object>()
                .Order(new HasOrderFactorComparer())
                .Cast<IAuthorizationHandler>());
    }

    public Task<IEnumerable<IAuthorizationHandler>> GetHandlersAsync(AuthorizationHandlerContext context)
        => _handlersTask;
}