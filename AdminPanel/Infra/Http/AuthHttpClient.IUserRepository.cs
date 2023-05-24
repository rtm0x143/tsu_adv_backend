using AdminPanel.Entities;
using AdminPanel.Services;
using AdminPanel.ViewModels;
using Common.Domain.ValueTypes;
using OneOf;

namespace AdminPanel.Infra.Http;

public partial class AuthHttpClient : IUserRepository
{
    public Task<OneOf<User, Exception>> Get(Guid id)
    {
        return SendAsJsonCheckedAsync<User>(HttpMethod.Get, "user/" + id);
    }

    public Task<OneOf<User[], Exception>> Get(UsersQuery query)
    {
        var queryString = query.Pagination.ToQueryString()
            .Add(nameof(query.InRestaurant), (query.InRestaurant ?? default).ToString());
        if (query.InRole != null)
            queryString = queryString.Add(nameof(query.InRole), query.InRole);

        return SendAsJsonCheckedAsync<User[]>(HttpMethod.Get, "user" + queryString);
    }

    public Task<OneOf<EmptyResult, Exception>> Delete(Guid id)
    {
        return SendCheckedAsync(HttpMethod.Delete, "user/" + id);
    }
}