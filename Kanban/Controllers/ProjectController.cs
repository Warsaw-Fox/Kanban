using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;

public class ProjectController : Controller
{
    public IActionResult Index()
    {
       return View(new ProjectInfo() { Name = "Kanban" });
    }
}
