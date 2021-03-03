using System;
using CmgLogParser.Domain;

namespace CmgLogParser.Sensors
{
    public class SensorFactory
    {
        private readonly string[] _referenceValues;

        public SensorFactory(string refValues)
        {
            _referenceValues = refValues.Split(" ");
        }

        //consider try get sensor
        public ISensor GetSensor(string? line)
        {
            //todo validation
            var record = line.Split(" ");
            switch (record[0])
            {
                case "thermometer":
                    return new Thermometer(record[1], double.Parse(_referenceValues[0]));
                case "humidity":
                    return new Humidity(record[1], double.Parse(_referenceValues[1]));
                case "monoxide":
                    return new Monoxide(record[1], int.Parse(_referenceValues[2]));
            }

            // Invalid sensor definition 
            throw new ApplicationException();
        }

        public bool TryGetSensor(string? line, out ISensor sensor)
        {
            //init default sensor 
            sensor = null;
            try
            {
                var record = line.Split(" ");
                switch (record[0])
                {
                    case "thermometer":
                        sensor = new Thermometer(record[1], double.Parse(_referenceValues[1]));
                        return true;
                    case "humidity":
                        sensor = new Humidity(record[1], double.Parse(_referenceValues[2]));
                        return true;
                    case "monoxide":
                        sensor = new Monoxide(record[1], int.Parse(_referenceValues[3]));
                        return true;
                    default:
                        throw new ApplicationException($"Sensor type with name {record[0]} not available");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}