using FootballAnalytics.Infrastructure.Configuration;
using FootballAnalytics.Web;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCommonConfiguration();

var matchCenterSettings = new MatchCenterConfiguration();  
new ConfigureFromConfigurationOptions<MatchCenterConfiguration>(builder.Configuration.GetSection("MatchCenterSettings")).Configure(matchCenterSettings);  
builder.Services.AddSingleton(matchCenterSettings);

var connectionString = ConnectionStringConfiguration.FromConfiguration(builder.Configuration);
builder.Services.AddSingleton(connectionString);

builder.Services.RegisterServices();

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