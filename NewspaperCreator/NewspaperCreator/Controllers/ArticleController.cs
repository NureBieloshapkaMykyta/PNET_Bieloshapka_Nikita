using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewspaperCreator.Controllers;

[Authorize]
public class ArticleController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
