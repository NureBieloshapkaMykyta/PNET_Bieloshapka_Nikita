using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewspaperCreator.Controllers;

[Authorize]
public class NewspaperController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
