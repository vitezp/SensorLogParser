using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using CmgLogParser.Domain;
using CmgLogParser.Sensors;

namespace CmgLogParser
{
    //suggestion for improvement - each line has first entry specification whether it's entry or sensor name or reference
    public interface ILogParser
    {
        public static string EvaluateLogFile(string logContentsStr)
        {
            //todo validate input
            using TextReader sr = new StringReader(logContentsStr);

            // always at the first line
            var reference = sr.ReadLine();

            // always at the second line
            var sensorFactory = new SensorFactory(reference);
            var valid = sensorFactory.TryGetSensor(sr.ReadLine(), out var sensor);

            var sensors = new List<ISensor>();
            var tasks = new List<Task<string>>();
            do
            {
                var line = sr.ReadLine();
                if (line == null)
                {
                    tasks.Add(sensor.Evaluate());
                    sensors.Add(sensor);
                    break;
                }

                var split = line.Split(" ");
                if (DateTime.TryParse(split[0], out var dateTime))
                {
                    valid &= sensor.TryAddEntry(dateTime, split[1]);
                }
                else
                {
                    Console.WriteLine($"evaluating {sensor.Name}");
                    tasks.Add(sensor.Evaluate());
                    sensors.Add(sensor);
                    valid &= sensorFactory.TryGetSensor(line, out sensor);
                }
            } while (valid);

            Console.WriteLine("exiting");

            Task.WaitAll(tasks.ToArray());

            return ProcessResult(sensors);
        }

        static string ProcessResult(IEnumerable<ISensor> sensors)
        {
            var dict = sensors.ToDictionary(m => m.Name, m => m.Result);
            return JsonSerializer.Serialize(dict, new JsonSerializerOptions {WriteIndented = true});
        }
    }
}