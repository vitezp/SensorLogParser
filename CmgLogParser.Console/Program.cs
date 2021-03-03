using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CmgLogParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "UnparsedLog.txt");
            var text = File.ReadAllText(file);
            var sw = new Stopwatch();
            sw.Start();
            System.Console.WriteLine(ILogParser.EvaluateLogFile(text));
            sw.Stop();
            System.Console.WriteLine($"{sw.Elapsed}");
        }
    }
}