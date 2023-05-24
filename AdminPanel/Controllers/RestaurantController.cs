using AdminPanel.Entities;
using AdminPanel.Services;
using AdminPanel.ViewModels;
using Common.App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

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

    [HttpGet]
    public async Task<IActionResult> Details(Guid? id)
    {
        var model = new EntityEditorViewModel { OnDeleteAction = "/Restaurant/Delete/" + id };

        if (!id.HasValue)
        {
            model.Entity = new Restaurant();
            return View("EntityEditor", model);
        }

        var result = await _repository.Get(id.Value).ConfigureAwait(false);
        if (!result.Succeeded()) return await ErrorView(result.Error());
        model.Entity = result.Value();

        return View("EntityEditor", model);
    }

    [HttpPost]
    public async Task<IActionResult> Details([FromRoute] Guid id, [FromForm(Name = "Entity")] Restaurant restaurant)
    {
        var result = id == default
            ? (await _repository.Create(restaurant)).MapT0(
                idResult =>
                {
                    id = idResult.Id;
                    return new EmptyResult();
                })
            : await _repository.Update(restaurant);
        if (!result.Succeeded()) return await ErrorView(result.Error());

        return RedirectToAction("Details", new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (id == default) return RedirectToAction("Index");
        var result = await _repository.Delete(id);
        if (result.Succeeded()) return RedirectToAction("Index");
        return await ErrorView(result.Error());
    }
}