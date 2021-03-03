using System;
using System.Linq;
using System.Threading.Tasks;

namespace CmgLogParser.Sensors
{
    public class Monoxide : Sensor
    {
        public Monoxide(string name, double reference)
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
                    return "precise";
                }

                var sum = values.Sum(d => (d - average) * (d - average));
                Result = Math.Sqrt(sum / values.Count) <= 3 ? "ultra precise" : "very precise";
                return Result;
            });        }
    }
}