using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Core.Entities;
using Shared.Helpers;

namespace NewspaperCreator.Controllers;

[Authorize]
public class IssueController : Controller
{
    private readonly IIssueService _issueService;

    public IssueController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _issueService.GetAllAsync();
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
    public async Task<IActionResult> Create(Issue issue)
    {
        if (ModelState.IsValid)
        {
            var result = await _issueService.CreateAsync(issue);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(issue);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(issue);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _issueService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Issue issue)
    {
        if (id != issue.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _issueService.UpdateAsync(issue);
            if (!result.IsSuccessful)
            {
                ModelState.AddModelError("", result.Message);
                return View(issue);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(issue);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _issueService.GetByIdAsync(id);
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
        var result = await _issueService.DeleteAsync(id);
        if (!result.IsSuccessful)
        {
            return Problem(result.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _issueService.GetByIdAsync(id);
        if (!result.IsSuccessful)
        {
            return NotFound();
        }
        return View(result.Data);
    }
}
