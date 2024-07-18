
using Kanban.Data;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

public class AuditLogFilterAttribute : Attribute, IActionFilter
{
    private readonly KanbanContext kanbanContext;

    public AuditLogFilterAttribute(KanbanContext kanbanContext)
    {
        this.kanbanContext = kanbanContext;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var identity = context.HttpContext.User.Identity;
        var request = context.HttpContext.Request;

        kanbanContext.AuditLogs.Add(new AuditLog
        {
            IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
            Url = request.Path + request.QueryString,
            Timestamp = DateTime.Now,
            User = identity.IsAuthenticated ? identity.Name : "Anonymous",
        });
        kanbanContext.SaveChanges();
    }
}
