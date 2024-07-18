using Kanban.Models;

namespace Kanban.Common;

public class Consts
{
    public static Dictionary<IssueState, string> IssueStates { get; } = new Dictionary<IssueState, string>
    {
        { IssueState.Todo, "do zrobienia"},
        { IssueState.Doing, "robione"},
        { IssueState.Done, "zrobione"}
    };
}

