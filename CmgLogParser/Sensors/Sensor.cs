using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmgLogParser.Sensors
{
    public abstract class Sensor
    {
        public string Name { get; set; }
        public double Reference { get; set; }

        protected List<Entry> Entries { get; set; } = new List<Entry>();

        public string Result { get; set; }

        public void AddEntry(DateTime date, string value)
        {
            Entries.Add(new Entry(date, value));
        }

        public abstract Task<string> Evaluate();
    }
}