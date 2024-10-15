using Cocona;
using Cocona.Builder;
using Gios.Extensions;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace Gios.Command;

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