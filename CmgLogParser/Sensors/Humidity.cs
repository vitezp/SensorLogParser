using System;
using System.Linq;
using System.Threading.Tasks;

namespace CmgLogParser.Sensors
{
    public class Humidity : Sensor
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
                var average = values.Average();
                if (Math.Abs(average - Reference) > 1)
                {
                    Result = "discard";
                    return Result;
                }

                Result = "keep";
                return Result;
            });
        }
    }
}