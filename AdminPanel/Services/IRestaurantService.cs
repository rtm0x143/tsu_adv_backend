using AdminPanel.Models;
using Common.Domain.ValueTypes;
using OneOf;

namespace AdminPanel.Services;

public interface IRestaurantRepository
{
    Task<OneOf<IEnumerable<Restaurant> , Exception>> Get(RestaurantQuery paginationInfo);
    Task<OneOf<EmptyResult, Exception>> Create(Restaurant restaurant);
}