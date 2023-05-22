using System.Collections;
using AdminPanel.Models;
using AdminPanel.Services;
using Common.Domain.ValueTypes;
using Common.Infra.HttpClients;
using Microsoft.AspNetCore.Http.Extensions;
using OneOf;
using static System.String;

namespace AdminPanel.Infra.Http;

public partial class BackendHttpClient : IRestaurantRepository
{
    public const string Name = "Backend";

    private readonly HttpClient _httpClient;
    public BackendHttpClient(IHttpClientFactory factory) => _httpClient = factory.CreateClient(Name);

    public Task<OneOf<IEnumerable<Restaurant>, Exception>> Get(RestaurantQuery paginationInfo)
    {
        var uri = new Uri("restaurant" + new QueryBuilder().AddObject(paginationInfo), UriKind.Relative);
        return _httpClient.SendAsJsonCheckedAsync<IEnumerable<Restaurant>>(HttpMethod.Get, uri);
    }

    public Task<OneOf<EmptyResult, Exception>> Create(Restaurant restaurant)
    {
        throw new NotImplementedException();
    }
}
