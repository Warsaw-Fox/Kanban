using Kanban.Common;
using Kanban.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Kanban.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddDbContext<KanbanContext>(options =>
{
    var appSettings = builder.Configuration.Get<AppSettings>();
    var sqlProvider = appSettings.SqlProvider; // Configuration.GetValue("SqlProvider", "sqlserver");
    Console.WriteLine($"appsettings.SqlProvider: {sqlProvider}");
    if (sqlProvider == "sqlite")
    {
        options.UseSqlite(builder.Configuration.GetConnectionString($"KanbanContext_{sqlProvider}"));
    }
    else
    {
        sqlProvider = "sqlserver";
        options.UseSqlServer(builder.Configuration.GetConnectionString($"KanbanContext_{sqlProvider}"));
    }
    Console.WriteLine($" '{sqlProvider}' database provider is used");
});


//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<KanbanIdentityDbContext>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<KanbanContext>();

// builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options => options.Filters.Add(typeof(AuditLogFilterAttribute)))
//.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
//.AddNewtonsoftJson();
.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

var app = builder.Build();



// ---------------------------------------------------------
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// - definicja routingu jedynie dla obszaru "Documentation"
app.MapAreaControllerRoute(name: "areas", areaName: "Documentation", pattern: "Documentation/{controller=Home}/{action=Index}/{id?}");

// - definicja routingu dla dowolnego obszaru poczatek sciezki to nazwa obszaru
// app.MapControllerRoute(name: "areas2", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// - definicja routingu gdzie adresy "documentation" lub "docs" lub "dokumentacja" prowadza do obszaru dokumentacji
// app.MapControllerRoute(name: "areas3", pattern: "{area_}/{controller=Home}/{action=Index}/{id?}",
//     new { area = "Documentation" }, new { area_ = "^(documentation|docs|dokumentacja)$" });

app.MapControllerRoute(name: "default", pattern: "{controller=Project}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    DatabaseUtils.InitDatabase(scope.ServiceProvider);
}
app.Run();








