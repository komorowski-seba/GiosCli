using Microsoft.Extensions.Configuration;

namespace Gios.Extensions;

public static class GiosSettingsExtension
{
    public static Settings.Gios GetGiosSettings(this IConfiguration conf) =>
         conf.GetSection(Settings.Gios.Name).Get<Settings.Gios>();
}