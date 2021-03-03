using System;
using System.Linq;
using System.Threading.Tasks;
using CmgLogParser.Domain;

namespace CmgLogParser.Sensors
{
    public class Humidity : Sensor<double>
    {
        public Humidity(string name, double reference)
        {
            Name = name;
            Reference = reference;
        }

        public override Task<string> Evaluate()
        {
            return Task.Run(() =>
            {
                var values = Entries.Select(m => m.Value).ToList();
                var matchesCriteria = values.All(m => Math.Abs(m - Reference) <= 1.0);

                Result = matchesCriteria ? "keep" : "discard";
                Console.WriteLine($"executing {Name}");

                return Result;
            });
        }

        public override bool TryAddEntry(DateTime date, string value)
        {
            var success = double.TryParse(value, out var parsed);
            if (success)
            {
                Entries.Add(new Entry<double>(date, parsed));
            }

            return success;
        }
    }
}