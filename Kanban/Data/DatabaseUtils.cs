
using Kanban.Common;
using Kanban.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kanban.Data;

public class DatabaseUtils
{
    public static void InitDatabase(IServiceProvider serviceProvider)
    {
        var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>();
        var resetDatabase = appSettings.Value.ResetDatabase;
        // var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        // var resetDatabase = configuration.GetValue("ResetDatabase", false);

        try
        {
            var newDatabaseWasCreated = RecreateDatabase(serviceProvider, resetDatabase);
            if (newDatabaseWasCreated)
            {
                InsertDefaultData(serviceProvider);
            }
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<DatabaseUtils>>();
            logger.LogError(ex, "An error occurred initializing the DB.");
        }
    }

    private static bool RecreateDatabase(IServiceProvider serviceProvider, bool deleteDatabaseIfExists)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<DatabaseUtils>>();

        using (var context = new KanbanContext(
            serviceProvider.GetRequiredService<DbContextOptions<KanbanContext>>()))
        {
            if (deleteDatabaseIfExists && context.Database.EnsureDeleted())
            {
                logger.LogInformation("Old database has been deleted");
            }
            if (context.Database.EnsureCreated())
            {
                logger.LogInformation("New database has been created");
                logger.LogInformation(context.Database.GenerateCreateScript());
                return true;
            }
            else
            {
                return false;
            }
            // uruchomienie brakujacych migracji
            // context.Database.Migrate();
        }
    }

    private static void InsertDefaultData(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<DatabaseUtils>>();

        using (var context = new KanbanContext(
            serviceProvider.GetRequiredService<DbContextOptions<KanbanContext>>()))
        {
            if (context.People.Any())
            {
                return;
            }

            var people = new List<Person>()
                {
                    new Person() { Name = "Gosia" },
                    new Person() { Name = "Marcin" },
                    new Person() { Name = "Lukasz" }
                };
            var issues = new List<Issue>()
            {
            };

            context.People.AddRange(people);
            context.Issues.AddRange(issues);
            context.SaveChanges();

            logger.LogInformation($"Default data has been added ({people.Count} people, {issues.Count} issues)");
        }
    }
}
