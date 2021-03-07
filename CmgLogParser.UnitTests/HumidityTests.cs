using System.Text;
using CmgLogParser.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace CmgLogParser.UnitTests
{
    public class Humidity
    {
        [Theory]
        [InlineData(45, 45, "h1", Result.Keep)]
        [InlineData(6.6, 6.3, "h2", Result.Keep)]
        [InlineData(2.2, 3.2, "h3", Result.Keep)]
        [InlineData(-2.2, -3.1, "h4", Result.Keep)]
        [InlineData(6, 7.1, "h5", Result.Discard)]
        [InlineData(6, 12, "h6", Result.Discard)]
        public void EvaluateLogFile_ShouldReadHumidityData_WhenTheLogfileIsValid(double reference, double value,
            string name, Result result)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(Common.ReferenceFormat, reference);
            sb.AppendFormat(Common.SensorFormat, SensorType.Humidity, name);
            sb.AppendFormat(Common.RecordFormat, value);
            var expected = Common.FormatResult(name, result);

            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }
    }
}