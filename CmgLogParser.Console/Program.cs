using System.Diagnostics;
using CmgLogParser.Domain.Enums;
using CmgLogParser.Sensors;
using CmgLogProducer;

namespace CmgLogParser.Console
{
    internal static class Program
    {
        
        // TODO prepare some command interface?
        private static void Main(string[] args)
        {
            var producer = new LogProducer();
            var produced = producer.ProduceSensorLog(SensorType.Thermometer, 6, 1000, 6);
            //var produced = producer.ProduceSensorLog(SensorType.Monoxide, 6, 1000, 6);
            //var produced = producer.ProduceSensorLog(SensorType.Humidity, 6, 1000, 10);

            var sw = new Stopwatch();
            sw.Start();
            var result = ILogParser.EvaluateLogFile(produced);
            System.Console.WriteLine(result);
            sw.Stop();
            System.Console.WriteLine($"{sw.Elapsed}");

            //File.WriteAllText("c:\\CODE\\Excercises\\Rider\\CmgLogParser\\CmgLogParser.UnitTests\\TestInputs\\LargeMonoxide_Log2.txt", produced);
            //File.WriteAllText("c:\\CODE\\Excercises\\Rider\\CmgLogParser\\CmgLogParser.UnitTests\\TestInputs\\LargeMonoxide_Result2.txt", result);
        }
    }
}