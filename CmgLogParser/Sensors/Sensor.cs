using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain;
using CmgLogParser.Domain.Enums;

namespace CmgLogParser.Sensors
{
    /// <summary>
    /// Contains common functionality for derived sensors.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Sensor<T> : ISensor where T : IComparable<T>
    {
        public string Name { get; protected init; }
        public SensorType SensorType { get; protected init; }
        public Result Result { get; protected set; } = Result.NoData;

        protected T Reference { get; init; }
        protected List<Entry<T>> Entries { get; } = new();


        public abstract Task Evaluate(CancellationToken ct);

        public abstract bool TryAddEntry(DateTime date, string value);
    }
}