using System;
using System.Threading.Tasks;

namespace CmgLogParser.Domain
{
    public interface ISensor
    {
        string Name { get; set; }
        string Result { get; set; }
        Task<string> Evaluate();
        bool TryAddEntry(DateTime date, string value);
    }
}