using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Models;

public partial class Person
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nazwa")]
    [Remote("IsNameUnique", "People")]
    public string Name { get; set; }

    public List<Issue> Issues { get; set; } = new List<Issue>();
}