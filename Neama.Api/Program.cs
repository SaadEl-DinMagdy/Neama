#region Configration Service
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Repository.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region Configure Kestrel middleware
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#endregion

#region update DataBase
using var Scoped = app.Services.CreateScope();
var Services = Scoped.ServiceProvider;
var _Dbcontext = Services.GetRequiredService<StoreContext>();

try
{
    await _Dbcontext.Database.MigrateAsync();

    var _UserManger = Services.GetRequiredService<UserManager<AppUser>>();

}
catch (Exception ex)
{

    var logger = Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
} 
#endregion

app.Run();
