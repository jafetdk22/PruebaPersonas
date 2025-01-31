using Microsoft.EntityFrameworkCore;
using PruebaApi.Models;
using PruebaApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PruebaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
    );
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IPersonaServices, PersonaServices>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html", permanent: false);
        return;
    }
    await next();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
