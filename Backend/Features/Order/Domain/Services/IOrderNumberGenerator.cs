namespace Backend.Features.Order.Domain.Services;

public interface IOrderNumberGenerator
{
    ValueTask<ulong> NextOrderNumber();
}