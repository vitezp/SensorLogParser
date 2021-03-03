using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain;

namespace CmgLogParser.Sensors
{
    public class Monoxide : Sensor<int>
    {
        public Monoxide(string name, int reference)
        {
            Name = name;
            Reference = reference;
        }

        public override Task<string> Evaluate()
        {
            return Task.Run(() =>
            {
                var values = Entries.Select(m => m.Value).ToList();


                var matchesCriteria = values.All(m => Math.Abs(m - Reference) <= 3.0);

                Result = matchesCriteria ? "keep" : "discard";
                Console.WriteLine($"executed {Name}");
                return Result;
            });
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