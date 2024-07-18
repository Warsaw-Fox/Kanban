using System.ComponentModel.DataAnnotations;

namespace Kanban.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string User { get; set; }
    public string IP { get; set; }
    public string Url { get; set; }
    public DateTime Timestamp { get; set; }
}