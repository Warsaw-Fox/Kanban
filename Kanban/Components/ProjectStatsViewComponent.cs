using Kanban.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Components;

[ViewComponent]
public class ProjectStatsViewComponent : ViewComponent
{
    private readonly KanbanContext kanbanContext;

    public ProjectStatsViewComponent(KanbanContext kanbanContext)
    {
        this.kanbanContext = kanbanContext;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var q =
            from issue in this.kanbanContext.Issues
            group issue by issue.State into issueStates
            select new { IssueState = issueStates.Key, Count = issueStates.Count() };

        var projectStats = await q.ToListAsync();
        var projectStatsModel = projectStats.ToDictionary(stat => stat.IssueState, stat => stat.Count);

        return View(projectStatsModel);
    }
}