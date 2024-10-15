using System.Net.Http.Json;
using Gios.Models;
using Spectre.Console;

namespace Gios.Extensions;

public static class SpectreExtension
{
    public static Task<List<Station>> GetStations(this HttpClient http, Settings.Gios giosConf) =>
        AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Default)
            .StartAsync<List<Station>>(
                "Get all stations...", 
                async ctx => 
                    await http.GetFromJsonAsync<List<Station>>(giosConf.AllStations) 
                    ?? []);
    
    public static Station GetSelectStation(this List<Station> allStations) =>
        AnsiConsole
            .Prompt(
                new SelectionPrompt<Station>()
                    .Title("Select station:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more station)[/]")
                    .AddChoices(allStations.OrderBy(n => n.Id).ToArray())
                    .UseConverter(n => $"({n.Id}) {n.StationName}"));

    public static Task<StationIndex> GetStationIndex(this HttpClient http, Station selected, Settings.Gios giosConf) =>
        AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Default)
            .StartAsync<StationIndex>(
                "Get all stations...", 
                async ctx => 
                    await http.GetFromJsonAsync<StationIndex>($"{giosConf.GetStationIndex}{selected.Id}") 
                    ?? new StationIndex());
    
    public static void ShowStationIndex(this StationIndex index)
    {
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn(index.Id.ToString());

        table.AddRow("Date", $"[green]{index.StCalcDate}[/]");
        table.AddRow("Index id", $"[red]{index.StIndexLevel.Id.ToString()}[/]");
        table.AddRow("Index level", $"[green]{index.StIndexLevel.IndexLevelName}[/]");

        AnsiConsole.Write(table);
    }
}