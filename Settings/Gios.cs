namespace Gios.Settings;

public class Gios
{
    public static string Name => "gios";
    
    public string Url { get; set; } = string.Empty;
    public string AllStations { get; set; } = string.Empty;
    public string GetStationIndex { get; set; } = string.Empty;
}