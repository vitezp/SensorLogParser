using System;

namespace CmgLogParser.Domain
{
    /// <summary>
    /// One record from the log file with datetime and value (int/double/...)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Entry<T> where T : IComparable<T>
    {
        private DateTime DateTime { get; }

        public T Value { get; }

        public Entry(DateTime dateTime, T value)
        {
            DateTime = dateTime;
            Value = value;
        }
    }
}