using Backend.Features.Basket.Commands;
using Backend.Infra.Data;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public partial class BasketController
    {
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpDelete]
        public Task<ActionResult> RemoveAll([FromServices] IClearBasket clearBasket)
        {
            if (Guid.TryParse(GetUserId(), out var userId)) return Task.FromResult(InvalidTokenPayload());
            return clearBasket.Execute(new(userId))
                .ContinueWith<ActionResult>(t => Ok());
        }
    }
}

namespace Backend.Features.Basket.Commands
{
    public class ClearBasket : IClearBasket
    {
        private readonly BackendDbContext _context;
        public ClearBasket(BackendDbContext context) => _context = context;

        public Task Execute(ClearBasketCommand request) => 
            _context.DishesInCart.Where(cart => cart.UserId == request.UserId).ExecuteDeleteAsync();
    }
}