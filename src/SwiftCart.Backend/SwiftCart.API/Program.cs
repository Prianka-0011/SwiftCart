
using EmotiaMart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SwiftCart.Application;
using SwiftCart.Infrastructure;
using SwiftCart.API.Middlewares;
using Microsoft.Extensions.FileProviders;
using SwiftCart.Domain.Entities;


var builder = WebApplication.CreateBuilder(args);

static void UpdateDatabase(IApplicationBuilder app)
{
    using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();

    using var context = serviceScope.ServiceProvider
        .GetService<AppDbContext>()
        ?? throw new Exception("LTRCDataContext service not found");
    context.Database.Migrate();
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

UpdateDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var uploadPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");

if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/uploads"
});

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        SwiftCart.Data.DbInitializer.Seed(context);

        var productCount = context.Products.Count();
        var categoryCount = context.Categories.Count();
        Console.WriteLine($"[DEBUG CHECK] Database contains {productCount} products and {categoryCount} categories.");

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
