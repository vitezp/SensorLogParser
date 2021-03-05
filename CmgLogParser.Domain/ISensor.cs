using System;
using System.Threading;
using System.Threading.Tasks;
using CmgLogParser.Domain.Enums;

namespace CmgLogParser.Domain
{
    /// <summary>
    /// Interface representing general sensor behavior
    /// </summary>
    public interface ISensor
    {
        /// <summary>
        /// Sensor name
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Type of a sensor
        /// </summary>
        SensorType SensorType { get; }

        /// <summary>
        /// If the sensor is evaluated, holds the verdicts for a particular sensor
        /// </summary>
        Result Result { get; }

        /// <summary>
        /// Fires a task to go thru all records for a particular sensor
        /// </summary>
        /// <returns></returns>
        Task Evaluate(CancellationToken ct);

        /// <summary>
        /// Inserts the pair of datetime and value to the list of entries for a particular sensor
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <param name="value">String representation of a value</param>
        /// <returns>Returns true if addition succeeded, otherwise false</returns>
        bool TryAddEntry(DateTime date, string value);
    }
}