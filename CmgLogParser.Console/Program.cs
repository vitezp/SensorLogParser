using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CmgLogParser.Domain.Enums;
using CmgLogProducer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CmgLogParser.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            
            // Unfortunate to have the static method in the interface so we can't use the DI to pass the logger to it :( 
            // This should definitely be done differently 
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            
            logger.LogDebug("Starting application");
            
            var logFiles = Directory.GetFiles(GetLogFilesDirectory()).ToList();
            var sw = new Stopwatch();

            System.Console.WriteLine("===========================================================");
            System.Console.WriteLine("=====================Log Analyzer==========================");
            System.Console.WriteLine("===========================================================");

            char pressed;
            do
            {
                System.Console.WriteLine(
                    "\nPress '1' to analyze the logs from file 'logFilesToAnalyze'.\nPress '2' to produce and then analyze a new log file.\nPress 'q' to exit");
                pressed = System.Console.ReadKey().KeyChar;
                string result;
                if (pressed == '1')
                {
                    foreach (var log in logFiles)
                    {
                        System.Console.WriteLine($"Evaluating logs from file {log}");
                        var logContent = File.ReadAllText(log);
                        sw.Restart();
                        result = ILogParser.EvaluateLogFile(logContent);
                        System.Console.WriteLine(result);
                        sw.Stop();
                        System.Console.WriteLine($"Elapsed: {sw.Elapsed}");
                    }
                }
                else if (pressed == '2')
                {
                    System.Console.WriteLine(
                        "\nPress '0' or '1' or '2' to generate logs for Thermometer(0), Monoxide(1) or Humidity(2) sensor. (Default = 0)");
                    if (!int.TryParse(System.Console.ReadKey().KeyChar.ToString(), out var sensor) || sensor > 2) break;
                    System.Console.WriteLine(
                        $"\nInsert number of sensors to generate log for. (Default = 5)");
                    var sensorCount = System.Console.ReadLine();
                    System.Console.WriteLine(
                        "Insert number of records to generate for each sensor. (Default = 100)");
                    var recordCount = System.Console.ReadLine();
                    System.Console.WriteLine(
                        "Insert a reference value. (Default = 25)");
                    var reference = System.Console.ReadLine();

                    var producer = new LogProducer();
                    var produced = producer.ProduceSensorLog((SensorType) int.Parse(sensor.ToString() ?? "0"),
                        int.Parse(sensorCount ?? "5"),
                        int.Parse(recordCount ?? "100"), int.Parse(reference ?? "25"));

                    System.Console.WriteLine(
                        "Press '0' or '1' to store the generated log into the file to logFilesToAnalyze folder. (Default = 0--don't store)");
                    var store = System.Console.ReadKey().KeyChar;
                    if (store.Equals('1'))
                    {
                        System.Console.WriteLine(
                            "Storing produced log as 'ProducedLogData.txt' to folder 'logFilesToAnalyze'");
                        File.WriteAllText(GetLogFilesDirectory() + "/ProducedLogData.txt", produced);
                    }

                    sw.Restart();
                    result = ILogParser.EvaluateLogFile(produced);
                    System.Console.WriteLine(result);
                    sw.Stop();
                    System.Console.WriteLine($"Elapsed: {sw.Elapsed}");
                }
            } while (pressed != 'f' && pressed != 'q');
            logger.LogDebug("All done!");
        }

        private static string GetLogFilesDirectory()
        {
            var a =
                Path.Combine(
                    Environment.CurrentDirectory ??
                    throw new InvalidOperationException("Directory does not exist"),
                    "logFilesToAnalyze");
            return a;
        }
    }
}