using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Core.Entities;
using Shared.Helpers;

namespace NewspaperCreator.Controllers;

[Authorize]
public class NewspaperController : Controller
{
    private readonly INewspaperService _newspaperService;

    public NewspaperController(INewspaperService newspaperService)
    {
        _newspaperService = newspaperService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _newspaperService.GetAllAsync();
        if (!result.IsSuccessful)
        {
            return Problem(result.Message);
        }
        return View(result.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Newspaper newspaper)
    {
        if (ModelState.IsValid)
        {
            var result = await _newspaperService.CreateAsync(newspaper);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(newspaper);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(newspaper);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _newspaperService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Newspaper newspaper)
    {
        if (id != newspaper.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _newspaperService.UpdateAsync(newspaper);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(newspaper);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(newspaper);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _newspaperService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _newspaperService.DeleteAsync(id);
        if (!result.IsSuccessful)
        {
            return Problem(result.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _newspaperService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }
}
