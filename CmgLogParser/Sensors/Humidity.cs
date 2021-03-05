using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain;
using CmgLogParser.Domain.Enums;

namespace CmgLogParser.Sensors
{
    public class Humidity : Sensor<double>
    {
        private readonly double _threshold;

        public Humidity(string name, double reference, double threshold = 1.0)
        {
            Name = name;
            Reference = reference;
            SensorType = SensorType.Humidity;
            _threshold = threshold;
        }

        public override Task Evaluate(CancellationToken ct)
        {
            return Task.Run(() =>
            {
                var values = Entries.Select(m => m.Value).ToList();
                if (values.Count == 0)
                {
                    Result = Result.NoData;
                    return;
                }

                var matchesCriteria = values.All(m => Math.Abs(m - Reference) <= _threshold);

                Result = matchesCriteria ? Result.Keep : Result.Discard;
                Console.WriteLine($"executing {Name}");
            }, ct);
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