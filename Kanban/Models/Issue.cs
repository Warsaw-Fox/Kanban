using System.ComponentModel.DataAnnotations;

namespace Kanban.Models;

public partial class Issue : IValidatableObject
{
    public int Id { get; set; }

    [Display(Name = "Tytuł")]
    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Display(Name = "Termin realizacji"), DataType(DataType.Date)]
    public DateTime? Deadline { get; set; }

    [Display(Name = "Stan")]
    public IssueState State { get; set; }

    [Display(Name = "Pilne")]
    public bool IsUrgent { get; set; }

    [Display(Name = "Wykonujący zadanie")]
    public int? AssignedToId { get; set; }
    public Person AssignedTo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Deadline.HasValue && Deadline.Value < DateTime.Now)
        {
            yield return new ValidationResult("Data wykonania zadania nie może być wcześniejszej niż aktualna data.", new[] { "Deadline" });
        }
    }
}



