using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain;
using CmgLogParser.Domain.Enums;

namespace CmgLogParser.Sensors
{
    public class Monoxide : Sensor<int>
    {
        private readonly int _threshold;

        public Monoxide(string name, int reference, int threshold = 3)
        {
            Name = name;
            Reference = reference;
            _threshold = threshold;
            SensorType = SensorType.Monoxide;
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
                Console.WriteLine($"executed {Name}");
            }, ct);
        }

        public override bool TryAddEntry(DateTime date, string value)
        {
            var success = int.TryParse(value, out var parsed);
            if (success)
            {
                Entries.Add(new Entry<int>(date, parsed));
            }

            return success;
        }
    }
}