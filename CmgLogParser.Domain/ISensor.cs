using System;
using System.Threading.Tasks;

namespace CmgLogParser.Sensors
{
    public interface ISensor
    {
        string Name { get; set; }
        string Result { get; set; }
        Task<string> Evaluate();
        void AddEntry(DateTime date, string value);
    }
}