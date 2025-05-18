using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Core.Entities;
using Shared.Helpers;

namespace NewspaperCreator.Controllers;

[Authorize]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _commentService.GetAllAsync();
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
    public async Task<IActionResult> Create(Comment comment)
    {
        if (ModelState.IsValid)
        {
            var result = await _commentService.CreateAsync(comment);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(comment);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(comment);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _commentService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Comment comment)
    {
        if (id != comment.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _commentService.UpdateAsync(comment);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(comment);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(comment);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _commentService.GetByIdAsync(id);
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
        var result = await _commentService.DeleteAsync(id);
        if (!result.IsSuccessful)
        {
            return Problem(result.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _commentService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }
}
