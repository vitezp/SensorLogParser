using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmgLogParser.Domain;
using CmgLogParser.Domain.Enums;
using CmgLogParser.Sensors;

namespace CmgLogProducer
{
    /// <summary>
    /// For testing purposes. This could be done more granularly but it serves the purpose. It' fairly efficient,
    /// able to generate millions of records when testing performance. 
    /// </summary>
    public class LogProducer
    {
        private readonly Random _rand;

        public LogProducer()
        {
            _rand = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Returns the log in a form of string based on the provided parameters
        /// </summary>
        /// <param name="sensor">Type of a sensor to generate date for</param>
        /// <param name="sensorCount">Number of sensors</param>
        /// <param name="recordCount">Number of records per sensor</param>
        /// <param name="reference">reference value</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string ProduceSensorLog(SensorType sensor, int sensorCount, int recordCount, int reference)
        {
            var count = 0;
            var sb = new StringBuilder();
            sb.AppendLine($"reference {reference} {reference} {reference}");

            while (count < sensorCount)
            {
                sb.AppendLine($"{sensor} {sensor}-{count}");
                GenerateSensorData(sensor, recordCount, reference, sb);
                count++;
            }

            return sb.ToString();
        }

        private void GenerateSensorData(SensorType sensor, int recordCount, int reference, StringBuilder sb)
        {
            for (var i = 0; i < recordCount; i++)
            {
                switch (sensor)
                {
                    case SensorType.Thermometer:
                    {
                        var seed = _rand.NextDouble() * 15;
                        var val = (seed - 7) + 5.0 / recordCount;
                        sb.AppendLine($"{DateTime.Now:s} {reference + val}");
                        break;
                    }
                    case SensorType.Monoxide:
                    {
                        var seed = _rand.Next(40 * 2);
                        var val = (seed - 40) / 10;
                        sb.AppendLine($"{DateTime.Now:s} {reference + val}");
                        break;
                    }
                    case SensorType.Humidity:
                    {
                        var seed = _rand.NextDouble() * 2;
                        var val = (seed - 1) + 0.5 / recordCount;
                        sb.AppendLine($"{DateTime.Now:s} {reference + val}");
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sensor), sensor, null);
                }
            }
        }
    }
}