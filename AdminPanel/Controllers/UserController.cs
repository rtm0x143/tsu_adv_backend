using AdminPanel.Converters;
using AdminPanel.Entities;
using AdminPanel.Services;
using AdminPanel.ViewModels;
using Common.App.Utils;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace AdminPanel.Controllers;

public class UserController : AdminPanelController
{
    private readonly IUserRepository _repository;
    public UserController(IUserRepository repository) => _repository = repository;

    public async Task<IActionResult> Index([FromQuery] UserCatalogViewModel model)
    {
        if (model.Query != null)
        {
            if ((await _repository.Get(model.Query)).TryGetValue(out var users, out var exception))
                model.Entities = users.Select(UserMapper.AdaptToPlainObject).ToArray();
            else return await ErrorView(exception);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (!id.HasValue)
            return await ErrorView(new NotImplementedException("User creation is no implemented in Admin Panel"));

        var result = await _repository.Get(id.Value).ConfigureAwait(false);
        if (!result.Succeeded()) return await ErrorView(result.Error());

        return View("EntityEditor", new EntityEditorViewModel
        {
            OnDeleteAction = "/User/Delete/" + id,
            Entity = result.Value().AdaptToPlainObject()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Details([FromRoute] Guid id, [FromForm(Name = "Entity")] UserProfile user,
        [FromForm(Name = "Entity.IsBanned")] bool isBanned,
        [FromServices] IProfileRepository profileRepository,
        [FromServices] IUserService userService)
    {
        var result = await profileRepository.UpdateProfile(id, user);
        if (!result.Succeeded()) return await ErrorView(result.Error());

        result = await userService.ChangeBanStatus(id, isBanned);
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