namespace Kanban.Common;

public class AppSettings
{
    public bool ResetDatabase { get; set; } = false;
    public string SqlProvider { get; set; } = "sqlserver";
}

