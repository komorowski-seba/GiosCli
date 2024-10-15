namespace Gios.Models;

public sealed class Station
{
    public int Id { get; set; }
    public string StationName { get; set; } = string.Empty;
    public string AddressStreet { get; set; } = string.Empty;
}