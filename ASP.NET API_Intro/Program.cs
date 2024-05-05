using ASP.NET_API_Intro.Models;
using ASP.NET_API_Intro.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Urls en minuscula
builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

// DbContext
builder.Services.AddDbContext<PokedexContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DbContext"));
});

// PokemonViewModel Service Layer 
builder.Services.AddScoped<PokemonViewModelService>();

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
app.Run();