using System.Text;
using CmgLogParser.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace CmgLogParser.Tests
{
    public class MonoxideTests
    {
        [Theory]
        [InlineData(45, 45, "m1", Result.Keep)]
        [InlineData(6, 9, "m2", Result.Keep)]
        [InlineData(-2, 1, "m3", Result.Keep)]
        [InlineData(6, 11, "m4", Result.Discard)]
        [InlineData(6, -1, "m5", Result.Discard)]
        public void EvaluateLogFile_ShouldReadMonoxideData_WhenTheLogfileIsValid(double reference, double value,
            string name, Result result)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(Common.ReferenceFormat, reference);
            sb.AppendFormat(Common.SensorFormat, SensorType.Monoxide, name);
            sb.AppendFormat(Common.RecordFormat, value);
            var expected = Common.FormatResult(name, result);

            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }
    }
}