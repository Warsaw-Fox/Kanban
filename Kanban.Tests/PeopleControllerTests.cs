using System.Runtime.CompilerServices;
using Kanban.Controllers;
using Kanban.Data;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Kanban.Tests;

[TestClass]
public class PeopleControllerTests
{
    [TestMethod]
    public async Task Index()
    {
        // Arrange
        var kanbanContext = CreateDbContext();
        kanbanContext.People.Add(new Person() { Id = 1, Name = "Marcin" });
        await kanbanContext.SaveChangesAsync();

        // Act
        var controller = new PeopleController(kanbanContext);
        var actionReslt = await controller.Index();

        // Assert
        var viewResult = Assert_IsType<ViewResult>(actionReslt);
        var model = Assert_IsType<List<Person>>(viewResult.Model);
        Assert.AreEqual(1, model.Count);
    }

    [TestMethod]
    public async Task Details_ReturnsView()
    {
        // Arrange
        var kanbanContext = CreateDbContext();
        kanbanContext.People.Add(new Person() { Id = 1, Name = "Marcin" });
        await kanbanContext.SaveChangesAsync();

        // Act
        var controller = new PeopleController(kanbanContext);
        var actionReslt = await controller.Details(1);

        // Assert
        var viewResult = Assert_IsType<ViewResult>(actionReslt);
        var model = Assert_IsType<Person>(viewResult.Model);
        Assert.AreEqual(1, model.Id);
        Assert.AreEqual("Marcin", model.Name);
    }


    [TestMethod]
    public async Task Details_ReturnsNotFound()
    {
        // Arrange
        var kanbanContext = CreateDbContext();

        // Act
        var controller = new PeopleController(kanbanContext);
        var actionReslt = await controller.Details(1);

        // Assert
        var viewResult = Assert_IsType<NotFoundResult>(actionReslt);
    }

    [TestMethod]
    public async Task Create_WithValidationError()
    {
        // Arrange
        var kanbanContext = CreateDbContext();

        // Act
        var controller = new PeopleController(kanbanContext);
        controller.ModelState.AddModelError("Name", "Required");
        var actionReslt = await controller.Create(new Person());

        // Assert
        var viewResult = Assert_IsType<ViewResult>(actionReslt);
    }


    [TestMethod]
    public async Task Create_Successfully()
    {
        // Arrange
        var kanbanContext = CreateDbContext();

        // Act
        var controller = new PeopleController(kanbanContext);
        var actionReslt = await controller.Create(new Person() { Id = 1, Name = "Marcin" });

        // Assert
        var redirectResult = Assert_IsType<RedirectToActionResult>(actionReslt);
        Assert.AreEqual("Index", redirectResult.ActionName);
        Assert.AreEqual(1, await kanbanContext.People.CountAsync());
    }


    private static KanbanContext CreateDbContext([CallerMemberName] string databaseName = "")
    {
        DbContextOptionsBuilder<KanbanContext> optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName)
            // https://stackoverflow.com/questions/44080733/how-to-suppress-inmemoryeventid-transactionignoredwarning-when-unit-testing-with
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        return new KanbanContext(optionsBuilder.Options);

    }

    private static T Assert_IsType<T>(object obj)
    {

        if (obj is T objOfT)
        {
            return objOfT;
        }
        else
        {
            Assert.Fail($"Object '{obj}' is not of type '{typeof(T)}'");
            return default(T);
        }
    }
}

