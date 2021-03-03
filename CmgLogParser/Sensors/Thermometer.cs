namespace CmgLogParser.Console.Models
{
    public abstract class Device
    {
        public string Name { get; set; }
    }

    public class Thermometer : Device
    {
    }

    public class Humidity : Device
    {
    }

    public class Monoxide : Device
    {
    }
}