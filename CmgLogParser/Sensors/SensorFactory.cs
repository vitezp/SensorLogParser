using System;

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
        public Sensor GetSensor(string? line)
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
                    return new Monoxide(record[1], double.Parse(_referenceValues[2]));
            }

            // Invalid sensor definition 
            throw new ApplicationException();
        }

        public bool TryGetSensor(string? line, out Sensor sensor)
        {
            //init default sensor 
            sensor = null;
            try
            {
                var record = line.Split(" ");
                switch (record[0])
                {
                    case "thermometer":
                        sensor = new Thermometer(record[1],double.Parse(_referenceValues[0]));
                        return true;
                    case "humidity":
                        sensor = new Thermometer(record[1],double.Parse(_referenceValues[1]));
                        return true;
        
                    case "monoxide":
                        sensor = new Thermometer(record[1],double.Parse(_referenceValues[2]));
                        return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        
            return false;
        }
    }
}