using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CmgLogParser.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace CmgLogParser.Tests
{
    public class ThermometerTests
    {
        [Theory]
        [InlineData(45, 45, "t1", Result.UltraPrecise)]
        [InlineData(6, 6, "t2", Result.UltraPrecise)]
        [InlineData(6.5, 6.5, "t3", Result.UltraPrecise)]
        [InlineData(10, 9.5, "t4", Result.UltraPrecise)]
        [InlineData(10, 11.0, "t6", Result.Precise)]
        [InlineData(10, 100.5, "t7", Result.Precise)]
        [InlineData(0, -0.5, "t7", Result.UltraPrecise)]
        public void EvaluateLogFile_ShouldReadThermometerSingleData_WhenTheLogfileIsValid(double reference,
            double value,
            string name, Result result)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(Common.ReferenceFormat, reference);
            sb.AppendFormat(Common.SensorFormat, SensorType.Thermometer, name);
            sb.AppendFormat(Common.RecordFormat, value);
            var expected = Common.FormatResult(name, result);

            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(45, 4, "t1", Result.VeryPrecise)]
        [InlineData(6, 4, "t2", Result.VeryPrecise)]
        [InlineData(6, 3.001, "t3", Result.VeryPrecise)]
        [InlineData(6, 5, "t4", Result.VeryPrecise)]
        public void EvaluateLogFile_ShouldReadThermometerVeryPrecise_WhenTheLogfileIsValid(double reference,
            double value,
            string name, Result result)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(Common.ReferenceFormat, reference);
            sb.AppendFormat(Common.SensorFormat, SensorType.Thermometer, name);
            sb.AppendFormat(Common.RecordFormat, reference + value);
            sb.AppendFormat(Common.RecordFormat, reference - value);
            sb.AppendFormat(Common.RecordFormat, reference + value);
            sb.AppendFormat(Common.RecordFormat, reference - value);
            var expected = Common.FormatResult(name, result);

            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }
    }
}