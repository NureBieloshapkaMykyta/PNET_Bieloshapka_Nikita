using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Core.Entities;
using Shared.Helpers;

namespace NewspaperCreator.Controllers;

[Authorize]
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _articleService.GetAllAsync();
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
    public async Task<IActionResult> Create(Article article)
    {
        if (ModelState.IsValid)
        {
            var result = await _articleService.CreateAsync(article);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(article);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(article);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _articleService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Article article)
    {
        if (id != article.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _articleService.UpdateAsync(article);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(article);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(article);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _articleService.GetByIdAsync(id);
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
        var result = await _articleService.DeleteAsync(id);
        if (!result.IsSuccessful)
        {
            return Problem(result.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _articleService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }
}
