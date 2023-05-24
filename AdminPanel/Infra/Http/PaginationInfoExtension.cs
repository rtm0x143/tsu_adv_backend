using Common.App.Dtos;

namespace AdminPanel.Infra.Http;

public static class PaginationInfoExtension
{
    public static QueryString ToQueryString<T>(this PaginationInfo<T> paginationInfo)
        => QueryString.Create("Pagination." + nameof(paginationInfo.PageSize), paginationInfo.PageSize.ToString())
            .Add("Pagination." + nameof(paginationInfo.AfterRecord),
                paginationInfo.AfterRecord?.ToString() ?? string.Empty);
}