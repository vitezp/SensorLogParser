![Github Actions Pipeline](https://github.com/vitezp/SensorLogParser/actions/workflows/config.yml/badge.svg)

#LogParser

The project uses .NET Core 5.0

## Repo structure

Solution is structured into 4 projects. 

`CmgLogParser`: Contains the interface and logic for parsing the logs.
**ILogParser**. Interface contains a method *EvaluateLogFile(string logContentsStr)* that takes
the log content and prints the output. 

`CmgLogParser.Domain`: Holds the common classes that could be reused for potential consumers
of the service. It could be either Console application or website.  

`CmgLogParser.Console`

`CmgLogParser.UnitTests`