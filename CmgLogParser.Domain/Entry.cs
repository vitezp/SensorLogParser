using System;

namespace CmgLogParser.Sensors
{
    public class Entry<T> where T : IComparable<T>
    {
        public DateTime DateTime { get; set; }

        public T Value { get; set; }

        public Entry(DateTime dateTime, T value)
        {
            DateTime = dateTime;
            Value = value;
        }
    }
}