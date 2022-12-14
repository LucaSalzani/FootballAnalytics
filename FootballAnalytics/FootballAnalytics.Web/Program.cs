using System.Runtime.CompilerServices;
using FootballAnalytics.Infrastructure.Configuration;
using FootballAnalytics.Web;

[assembly: InternalsVisibleTo("FootballAnalytics.Tests")]

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCommonConfiguration();

builder.Services.RegisterServices(builder.Configuration);

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