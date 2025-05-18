using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewspaperCreator.Controllers;

[Authorize]
public class ComentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
