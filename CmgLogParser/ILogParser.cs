using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain;

namespace CmgLogParser
{
    //suggestion for improvement of Logformat
    //  - each line has first entry specification whether it's entry or sensor name or reference
    public interface ILogParser
    {
        public static string EvaluateLogFile(string logContentsStr)
        {
            using TextReader sr = new StringReader(logContentsStr);
            var sensors = new List<ISensor>();

            // always at the first line
            var reference = sr.ReadLine();

            // always at the second line
            var sensorFactory = SensorFactory.CreateSensorFactory(reference);
            if (sensorFactory == null)
            {
                return Helpers.FormatResult(sensors);
            }

            var valid = sensorFactory.TryGetSensor(sr.ReadLine(), out var sensor);
            var tasks = new List<Task>();

            while (valid)
            {
                var line = sr.ReadLine();
                if (line == null)
                {
                    tasks.Add(sensor.Evaluate(CancellationToken.None));
                    sensors.Add(sensor);
                    break;
                }

                var split = line.Split(Constants.Separator);
                valid &= DateTime.TryParse(split[0], out var dateTime);
                if (valid && split.Length == 2)
                {
                    valid &= sensor.TryAddEntry(dateTime, split[1]);
                }

                if (valid) continue;

                Console.WriteLine($"evaluating {sensor.Name}");
                tasks.Add(sensor.Evaluate(CancellationToken.None));
                sensors.Add(sensor);
                valid = sensorFactory.TryGetSensor(line, out sensor);
            }

            Console.WriteLine("exiting");

            Task.WaitAll(tasks.ToArray());

            return Helpers.FormatResult(sensors);
        }
    }
}