namespace AdminPanel.Views.Shared.DisplayTemplates;

public record PropertyTableViewData(string? IdPropertyName, string? GoToItemController, string? GoToItemAction,
    int StartNumberingFrom = 1)
{
    public PropertyTableViewData(IDictionary<string, object?> viewData) : this(
        IdPropertyName: viewData[nameof(IdPropertyName)] as string,
        GoToItemController: viewData[nameof(GoToItemController)] as string,
        GoToItemAction: viewData[nameof(GoToItemAction)] as string)
    {
        if (viewData[nameof(StartNumberingFrom)] is int startFrom) StartNumberingFrom = startFrom;
    }
}