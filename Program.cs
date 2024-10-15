using Cocona;
using Gios.Command;
using Gios.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();
builder.Services.AddScoped<HttpClient>(o =>
{
    var conf = o.GetService<IConfiguration>();
    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri(conf!.GetGiosSettings().Url);
    return httpClient;
});
builder.Services.AddScoped<IConfiguration>(_ =>
{
    var builderConf = new ConfigurationBuilder();
    builderConf
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    return builderConf.Build();
});


var app = builder.Build();
app.AddGetAllStationsCommand();
app.AddSelectStationCommand();
app.Run();