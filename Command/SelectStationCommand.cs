using Cocona;
using Cocona.Builder;
using Gios.Extensions;
using Gios.Models;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace Gios.Command;

public static class SelectStationCommand
{
    private static string Name => "select";
    
    public static ICoconaCommandsBuilder AddSelectStationCommand(this ICoconaCommandsBuilder builder)
    {
        builder.AddCommand(
            Name, 
            async ([Argument(Description = "station id")] int sId, HttpClient http, IConfiguration conf) =>
            {
                var giosConf = conf.GetGiosSettings();
                var stationIndex = await http.GetStationIndex(new Station {Id = sId}, giosConf);
                AnsiConsole.Console.Clear();
                stationIndex.ShowStationIndex();
        });
        return builder;
    }
}