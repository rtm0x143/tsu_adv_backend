namespace AdminPanel.Views.Shared.DisplayTemplates;

public record PropertyTableViewData(string? IdPropertyName, string? GoToItemController, string? GoToItemAction)
{
    public PropertyTableViewData(IDictionary<string, object?> viewData) : this(
        IdPropertyName: viewData[nameof(IdPropertyName)] as string,
        GoToItemController: viewData[nameof(GoToItemController)] as string,
        GoToItemAction: viewData[nameof(GoToItemAction)] as string)
    {
    }
}