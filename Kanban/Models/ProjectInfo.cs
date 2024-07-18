using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban.Models;

public class ProjectInfo
{
    public int ProjectInfoId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Column("Start")]
    public DateTime StartDate { get; set; }

    [Column("Finish")]
    public DateTime? FinishDate { get; set; }
}