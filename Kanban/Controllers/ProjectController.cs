using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Data;

namespace Kanban.Controllers;

public class ProjectController : Controller
{
    private readonly KanbanContext kanbanContext;

    public ProjectController(KanbanContext kanbanContext)
    {
        this.kanbanContext = kanbanContext;
    }

    public IActionResult Index()
    {
        return View(new ProjectInfo() { Name = "Kanban" });
    }


    public IActionResult Read()
    {
        var result =
            from p in this.kanbanContext.Project
            select p;

        return Json(result.ToList());
    }

    public IActionResult Create()
    {
        var project = new ProjectInfo
        {
            Name = "nowy projekt " + DateTime.Now.ToString(),
            StartDate = DateTime.Now,
            FinishDate = DateTime.Now.AddDays(10),
        };
        kanbanContext.Project.Add(project);
        kanbanContext.SaveChanges();

        return Content($"dodano nowy projekt z id '{project.ProjectInfoId}'");
    }

    public IActionResult Update()
    {
        var firstProject = this.kanbanContext.Project.FirstOrDefault();
        if (firstProject == null)
        {
            return Content("brak projektow");
        }
        if (firstProject.FinishDate.HasValue)
        {
            firstProject.FinishDate = firstProject.FinishDate.Value.AddDays(5);
        }

        this.kanbanContext.SaveChanges();

        return Content($"przesunieto termin zakonczenia projekt z id '{firstProject.ProjectInfoId}' o 5 dni");
    }

    public IActionResult Delete()
    {
        var firstProject = this.kanbanContext.Project.FirstOrDefault();
        if (firstProject == null)
        {
            return Content("brak projektow");
        }

        this.kanbanContext.Project.Remove(firstProject);
        this.kanbanContext.SaveChanges();

        return Content($"usunieto projekt z id '{firstProject.ProjectInfoId}'");
    }
}
