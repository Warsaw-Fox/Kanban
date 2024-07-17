namespace Kanban.Models;

public class ProjectInfo
{
    public string Name { get; set; }
}
public partial class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Issue> Issues { get; set; } = new List<Issue>();
}

public partial class Issue
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime? Deadline { get; set; }
    public IssueState State { get; set; }
    public bool IsUrgent { get; set; }

    public int? AssignedToId { get; set; }
    public Person AssignedTo { get; set; }
}

public enum IssueState { Todo, Doing, Done }

