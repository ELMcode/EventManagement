using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventManagement.Data;
using EventManagement.Models;
using EventManagement.Models.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    }));

// Configure MongoDB (WIP)
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
if (!string.IsNullOrEmpty(mongoConnectionString))
{
    var mongoClient = new MongoClient(mongoConnectionString);
    var mongoDatabase = mongoClient.GetDatabase("EventManagement");
    builder.Services.AddSingleton<IMongoClient>(mongoClient);
    builder.Services.AddScoped<IEventStatsRepository>(provider => new EventStatsRepository(mongoDatabase));
}

// Add Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();

var app = builder.Build();

// Initialize the database with admin user and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Event}/{action=Index}/{id?}");

app.Run();
