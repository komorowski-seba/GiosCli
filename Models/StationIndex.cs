namespace Gios.Models;

public class StationIndex
{
    public int Id { get; set; }
    public string StCalcDate { get; set; } = string.Empty;
    public StationIndexLevel StIndexLevel { get; set; } = new();
}