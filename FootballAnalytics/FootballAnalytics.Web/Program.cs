using FootballAnalytics.Infrastructure.Configuration;
using FootballAnalytics.Web;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;
    var configurationFolder = Path.Combine(env.ContentRootPath, "..", "Configuration");

    config.AddJsonFile(Path.Combine(configurationFolder, "common.appsettings.json"),
            optional: true) // When running using dotnet run
        .AddJsonFile("common.appsettings.json", optional: true); // When app is published
});

var matchCenterSettings = new MatchCenterConfiguration();  
new ConfigureFromConfigurationOptions<MatchCenterConfiguration>(builder.Configuration.GetSection("MatchCenterSettings")).Configure(matchCenterSettings);  
builder.Services.AddSingleton(matchCenterSettings);

var localConnectionTemplate = builder.Configuration.GetConnectionString("LocalSqliteConnection");
var connectionString = new ConnectionStringConfiguration
{
    LocalSqliteConnection = localConnectionTemplate.Replace("{PathToDbFile}", $"{Environment.GetEnvironmentVariable("HOME")}\\FootballAnalytics.db")
};

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