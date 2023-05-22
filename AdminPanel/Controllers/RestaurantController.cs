using AdminPanel.Models;
using AdminPanel.Services;
using Common.App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdminPanel.Controllers;

public class RestaurantController : AdminPanelController
{
    private readonly IRestaurantRepository _repository;
    public RestaurantController(IRestaurantRepository repository) => _repository = repository;

    public async Task<IActionResult> Index([FromQuery] RestaurantCatalogViewModel model)
    {
        if (ModelState.GetFieldValidationState(string.Join('.',
                nameof(RestaurantCatalogViewModel.Query),
                nameof(RestaurantCatalogViewModel.Query.Pagination),
                nameof(RestaurantCatalogViewModel.Query.Pagination.PageSize))) == ModelValidationState.Valid)
        {
            if (!(await _repository.Get(model.Query!))
                .TryGetValue(out var restaurants, out var exception))
                return await ErrorView(exception);
            
            model.Entities = restaurants.ToArray();
        }

        return View(model);
    }

    public IActionResult Details(Guid id)
    {
        // TODO
        return View("Index", new RestaurantCatalogViewModel());
    }
}