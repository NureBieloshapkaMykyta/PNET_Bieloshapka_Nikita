using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewspaperCreator.Controllers;

[Authorize]
public class IssueController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
