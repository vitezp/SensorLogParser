using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using CmgLogParser.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace CmgLogParser.UnitTests
{
    public class LogParserGeneralTests
    {
        [Theory]
        [InlineData("reference  2 2 2")]
        [InlineData("reference 2 2 abc")]
        [InlineData("refference 2 2 2")]
        [InlineData("ref\nerence 2 2 2")]
        [InlineData("reference 2 2")]
        [InlineData("reference 2 2  2")]
        [InlineData("reference 2 2  2.1")]
        [InlineData("2021-03-03T22:02 2")]
        public void EvaluateLogFile_ShouldReturnEmpty_WhenTheReferenceIsInvalid(string reference)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(reference);
            sb.AppendLine();
            sb.AppendFormat(Common.SensorFormat, SensorType.Monoxide, "m1");
            sb.AppendFormat(Common.RecordFormat, "2");
            const string expected = "{}"; //"invalid reference format";

            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("thermometer")]
        [InlineData("thermometerr name")]
        [InlineData("monoxide\n name")]
        [InlineData("\nmonoxide name")]
        [InlineData(" humidity name")]
        [InlineData("humidityy name")]
        public void EvaluateLogFile_ShouldReturnEmpty_WhenSensorIsInvalid(string sensorName)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(Common.ReferenceFormat, 3);
            sb.AppendFormat(sensorName);
            sb.AppendLine();
            sb.AppendFormat(Common.RecordFormat, "2");
            const string expected = "{}"; //"invalid sensor name format";

            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("2021-03-03T22:02 invalid")]
        [InlineData("2021-03-03T22:02  2")]
        [InlineData("2021-03-03T22:02\n2")]
        [InlineData("3/4/2021 8:14:03 PM 2")]
        public void EvaluateLogFile_ShouldReturnEmpty_WhenRecordIsInvalid(string record)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(Common.ReferenceFormat, 3);
            sb.AppendFormat(Common.SensorFormat, SensorType.Monoxide, "m1");
            sb.AppendFormat(record);
            string expected = Common.FormatResult("m1", Result.NoData);

            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(SensorType.Monoxide)]
        [InlineData(SensorType.Humidity)]
        [InlineData(SensorType.Thermometer)]
        public void EvaluateLogFile_ShouldReturnDefault_WhenNoRecordForSensor(SensorType sensor)
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendFormat(Common.ReferenceFormat, 3);
            sb.AppendFormat(Common.SensorFormat, sensor, "s1");
            var expected = Common.FormatResult("s1", Result.NoData);
            // Act
            var res = ILogParser.EvaluateLogFile(sb.ToString());

            // Assert
            res.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("LargeThermometer_Log.txt", "LargeThermometer_Result.txt")]
        [InlineData("Assignment_Log.txt", "Assignment_Result.txt")]
        [InlineData("LargeMonoxide_Log.txt", "LargeMonoxide_Result.txt")]
        [InlineData("LargeHumidity_Log.txt", "LargeHumidity_Result.txt")]
        public void Input(string input, string result)
        {
            // Arrange
            var inputData = File.ReadAllText(Directory.GetCurrentDirectory() + "/TestInputs/" + input);
            var resultData = File.ReadAllText(Directory.GetCurrentDirectory() + "/TestInputs/" + result);

            // Act
            var parsed = ILogParser.EvaluateLogFile(inputData);

            //Assert
            parsed.Should().BeEquivalentTo(resultData);
        }
    }
}