using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CmgLogParser.Domain;

namespace CmgLogParser
{
    public static class Helpers
    {
        internal static string FormatResult(IEnumerable<ISensor> sensors)
        {
            var dict = sensors.ToDictionary(m => m.Name, m => m.Result);
            return JsonSerializer.Serialize(dict, new JsonSerializerOptions {WriteIndented = true,});
        }
    }
}