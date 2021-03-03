using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmgLogParser.Domain;

namespace CmgLogParser.Sensors
{
    public abstract class Sensor<T> : ISensor where T : IComparable<T>
    {
        public string Name { get; set; }
        protected T Reference { get; set; }

        protected List<Entry<T>> Entries { get; set; } = new();

        public string Result { get; set; }

        public abstract Task<string> Evaluate();

        public abstract bool TryAddEntry(DateTime date, string value);
    }
}