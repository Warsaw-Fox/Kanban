using Microsoft.AspNetCore.Mvc;

namespace Kanban.Areas.Documentation.Controllers;

[Area("Documentation")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
