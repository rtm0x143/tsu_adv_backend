using AdminPanel.Entities;
using AdminPanel.Services;
using AdminPanel.ViewModels;
using Common.Domain.ValueTypes;
using Microsoft.AspNetCore.Http.Extensions;
using OneOf;

namespace AdminPanel.Infra.Http;

public partial class BackendHttpClient : IRestaurantRepository
{
    private const string RestaurantSegment = "restaurant";

    public Task<OneOf<IEnumerable<Restaurant>, Exception>> Get(RestaurantQuery paginationInfo)
    {
        var uri = new Uri("restaurant" + paginationInfo.Pagination.ToQueryString(), UriKind.Relative);
        return SendAsJsonCheckedAsync<IEnumerable<Restaurant>>(HttpMethod.Get, uri);
    }

    public Task<OneOf<Restaurant, Exception>> Get(Guid id)
    {
        return SendAsJsonCheckedAsync<Restaurant>(HttpMethod.Get, RestaurantSegment + '/' + id);
    }

    public record RestaurantCreation(string Name);

    public Task<OneOf<IdResult, Exception>> Create(Restaurant restaurant)
    {
        return SendAsJsonCheckedAsync<IdResult, RestaurantCreation>(HttpMethod.Post, RestaurantSegment,
            new(restaurant.Name));
    }

    public Task<OneOf<EmptyResult, Exception>> Update(Restaurant restaurant)
    {
        return SendAsJsonCheckedAsync(HttpMethod.Put, RestaurantSegment + '/' + restaurant.Id,
            payload: new RestaurantCreation(restaurant.Name));
    }

    public Task<OneOf<EmptyResult, Exception>> Delete(Guid id)
    {
        return SendCheckedAsync(HttpMethod.Delete, RestaurantSegment + '/' + id);
    }
}