using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain;

namespace CmgLogParser.Sensors
{
    public class Thermometer : Sensor<double>
    {
        public Thermometer(string name, double reference)
        {
            Name = name;
            Reference = reference;
        }

        public override Task<string> Evaluate()
        {
            return Task.Run(() =>
            {
                var values = Entries.Select(m => m.Value).ToList();
                var average = values.Average();
                if (Math.Abs(average - Reference) > 0.5)
                {
                    Result = "precise";
                    return Result;
                }

                var stdDev = Math.Sqrt(values.Sum(d => (d - average) * (d - average)) / values.Count);
                Result = stdDev switch
                {
                    <= 3.0 => "ultra precise",
                    <= 5.0 => "very precise",
                    _ => "precise"
                };
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine($"executed {Name}");

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