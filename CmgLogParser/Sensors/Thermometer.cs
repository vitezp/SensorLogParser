using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain;
using CmgLogParser.Domain.Enums;

namespace CmgLogParser.Sensors
{
    public class Thermometer : Sensor<double>
    {
        public Thermometer(string name, double reference)
        {
            Name = name;
            Reference = reference;
            SensorType = Domain.Enums.SensorType.Thermometer;
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

                var average = values.Average();
                if (Math.Abs(average - Reference) > 0.5)
                {
                    Result = Result.Precise;
                    return;
                }

                Thread.Sleep(1000);
                var stdDev = Math.Sqrt(values.Sum(d => (d - average) * (d - average)) / values.Count);
                Result = stdDev switch
                {
                    <= 3.0 => Result.UltraPrecise,
                    <= 5.0 => Result.VeryPrecise,
                    _ => Result.Precise
                };
                Console.WriteLine($"executed {Name}");
            }, ct);
        }

        public override bool TryAddEntry(DateTime date, string value)
        {
            if (value == null) return false;

            var success = double.TryParse(value, out var parsed);
            if (success)
            {
                Entries.Add(new Entry<double>(date, parsed));
            }

            return success;
        }
    }
}