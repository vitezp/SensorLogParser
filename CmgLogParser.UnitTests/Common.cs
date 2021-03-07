using System.Collections.Generic;
using System.Text.Json;
using CmgLogParser.Domain.Enums;

namespace CmgLogParser.UnitTests
{
    public static class Common
    {
        internal const string ReferenceFormat = "reference {0} {0} {0}\n"; // value
        internal const string SensorFormat = "{0} {1}\n"; // sensorType name
        internal const string RecordFormat = "2021-03-03T22:02 {0}\n"; // value

        internal static string FormatResult(string name, Result result)
        {
            var col = new Dictionary<string, Result> {{name, result}};
            return JsonSerializer.Serialize(col, new JsonSerializerOptions {WriteIndented = true});
        }
    }
}