using AdminPanel.Entities;
using AdminPanel.ViewModels;
using Common.Domain.ValueTypes;
using OneOf;

namespace AdminPanel.Services;

public interface IRestaurantRepository
{
    Task<OneOf<IEnumerable<Restaurant>, Exception>> Get(RestaurantQuery paginationInfo);
    Task<OneOf<Restaurant, Exception>> Get(Guid id);
    Task<OneOf<IdResult, Exception>> Create(Restaurant restaurant);
    Task<OneOf<EmptyResult, Exception>> Update(Restaurant restaurant);
    Task<OneOf<EmptyResult, Exception>> Delete(Guid id);
}