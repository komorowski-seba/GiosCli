![image](Imgs/gios_cl.gif)

When we wanting to make a nice and modern console application, in Net
we have some cool libraries, but got in my hands on 
[Spectre](https://github.com/spectreconsole/spectre.console) and [Cocona](https://github.com/mayuki/Cocona).

The **Cocona** allows in a very friendly way to
create commands line application.
But **Spectre** allows to you a build the nice interface for a console application.

The first step is the create a builder, next one to load services
into the DI container if is necessary

```c#
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
```

The next thing is to add a command,
This can be done in several ways, I chosed the
extension method

```c#
var app = builder.Build();
app.AddGetAllStationsCommand();
app.AddSelectStationCommand();
app.Run();
```

```c#
public static class GetAllStationsCommand
{
    private static string Name => "getAll";
    
    public static ICoconaCommandsBuilder AddGetAllStationsCommand(this ICoconaCommandsBuilder builder)
    {
        builder.AddCommand(Name, async (HttpClient http, IConfiguration conf) =>
        {
            var giosConf = conf.GetGiosSettings();
            var stations = await http.GetStations(giosConf);
            AnsiConsole.Console.Clear();
            var selectStation = stations.GetSelectStation();
            var stationIndex = await http.GetStationIndex(selectStation, giosConf);
            AnsiConsole.Console.Clear();
            stationIndex.ShowStationIndex();
        });
        return builder;
    }
}
```

Method **builder.AddCommand(Name, async (HttpClient http, IConfiguration conf) => {});**.
allows you to create a command with the appropriate name with the appropriate parameters, and
we can inject pre-loaded services from the dependency injection.

For **Spectre** I also made the extensions methods, providing the appropriate iteration

```c#
public static Station GetSelectStation(this List<Station> allStations) =>
    AnsiConsole
        .Prompt(
            new SelectionPrompt<Station>()
                .Title("Select station:")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more station)[/]")
                .AddChoices(allStations.OrderBy(n => n.Id).ToArray())
                .UseConverter(n => $"({n.Id}) {n.StationName}"));
```

The possibilities are really large, and you can create an
interesting-looking console application.