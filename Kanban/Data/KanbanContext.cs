
using Kanban.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Data;

public class KanbanContext
    : IdentityDbContext<IdentityUser>
// : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
    }
    public DbSet<Person> People { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<ProjectInfo> Project { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectInfo>().HasIndex(e => e.Name).IsUnique();
    }
}