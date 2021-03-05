using System;
using CmgLogParser.Domain;
using CmgLogParser.Domain.Enums;
using CmgLogParser.Sensors;

namespace CmgLogParser
{
    
    /// <summary>
    /// Contains definition on how to create sensors and holds the reference value
    /// </summary>
    public class SensorFactory
    {
        private readonly string[] _referenceValues;

        private SensorFactory(string[] refValues)
        {
            _referenceValues = refValues;
        }

        /// <summary>
        /// Try to parse the line and instantiate sensor from any defined.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public bool TryGetSensor(string line, out ISensor sensor)
        {
            sensor = null;

            var record = line.Split(Constants.Separator);
            if (record.Length != 2)
            {
                Console.WriteLine($"Invalid data record format: '{record}'. Expected: 'recordType recordName'");
                return false;
            }

            var success = Enum.TryParse(record[0], true, out SensorType parsed);
            if (!success)
            {
                Console.WriteLine(
                    $"Unable to parse sensor type from record. Available values: '{string.Join(", ", Enum.GetValues<SensorType>())}', Got: '{record[0]}");
                return false;
            }

            try
            {
                //TODO consider reflection and loading all the sensors form the given folder (stretch)
                switch (parsed)
                {
                    case SensorType.Thermometer:
                        sensor = new Thermometer(record[1], double.Parse(_referenceValues[1]));
                        return true;
                    case SensorType.Humidity:
                        sensor = new Humidity(record[1], double.Parse(_referenceValues[2]), 1.0);
                        return true;
                    case SensorType.Monoxide:
                        sensor = new Monoxide(record[1], int.Parse(_referenceValues[3]), 3);
                        return true;
                    default:
                        return false;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Invalid format of a reference value for sensor of type: {parsed}. \n{e}");
                return false;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(
                    $"Not enough reference values provided for given sensor types. Expected: '{Enum.GetValues<SensorType>().Length}', Got: {_referenceValues.Length - 1}\n{e}");
                return false;
            }
        }

        public static SensorFactory CreateSensorFactory(string reference)
        {
            var split = reference.Split(" ");

            if (split.Length - 1 != Enum.GetValues<SensorType>().Length)
            {
                Console.WriteLine("Not enough reference values provided");
                return null;
            }

            if (split[0].Equals(Constants.Reference, StringComparison.InvariantCultureIgnoreCase))
                return new SensorFactory(split);
            Console.WriteLine("Invalid definition of reference values");
            return null;
        }
    }
}