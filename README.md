![Github Actions Pipeline](https://github.com/vitezp/SensorLogParser/actions/workflows/dotnet.yml/badge.svg)

# LogParser

The project uses .NET Core 5.0

## Getting started
To build the solution
```
$ dotnet build
```
To run the simple console application for demo purposes:
```
$ cd CmgLogParser.Console/bin/Debug/net5.0/
$ dotnet CmgLogParser.Console.dll
```

## Algorithm description

Program reads the input line after line until it reaches EOF. When a new sensor is specified, LogParser instantiates the
object of the given sensor type and starts adding the records entries with DateTime and value until it reaches the new
sensor or EOF. When it does, the method Evaluate is triggered that executes the evaluation based on the rules defined for
a particular sensor type. This method is non-blocking thus the program keeps reading the file for new log records while
the sensor evaluation is in progress. Note that currently it does not yield a significant performance impact, as the
rules are fairly simple, but given we have more complex rules to compute the sensor result, the saved time can be pretty
significant. (Simulated with 40 sensors where evaluation took 1 second, parallel analysis finished in 4seconds where
sequential took more than 42s).

## New sensor addition

Potential future addition of a new sensor is fairly simple:

1. We need to add the sensor as a new class to the sensors folder into the *CmgLogParser.Domain* project that derives
   from ISensor.
3. In the new class we need to implement the method Evaluate.
2. Add necessary logic to the SensorFactory to specify how the new sensor is supposed to be created and what type of
   value it expects.

## Repo structure

Solution for LogParser is structured into 4 projects and one unrelated project used for testing purposes

`CmgLogParser`: Contains the interface and logic for parsing the logs.
**ILogParser**. Interface contains a method *EvaluateLogFile(string logContentsStr)* that takes the log content and
prints the output.

`CmgLogParser.Domain`: Holds the common classes that could be reused for potential consumers of the service. It could be
either Console application or website.

`CmgLogParser.Console` Really simple command line interface that has 2 main functions:</br>

1. Evaluating all the logfiles from the folder 'logFilesToAnalyze' and printing the results.
2. Generating new logfile and running the evaluation off of it. It also allows us to store the generated data to a
   text-file into 'logFilesToAnalyze' folder.

`CmgLogParser.UnitTests` Xunit test project covering all the functionality of the parser. Tests are run in parallel.

`CmgLogProducer` Simple class library that references the *CmgLogParser* and *CmgLogParser.Domain* to generate the
random log file for functionality checking and performance tuning purposes.

## Images

![Frontend](/images/consoleInterface.PNG?raw=true)

## Improvement suggestion / Further continuation
-Change the interface method to non-static to use DI. This would be much much better solution as oppose to having
the implementation in the interface itself which is super nasty. </br>
-Define the method that takes reader as oppose to string to continuously analyze logs and post the result.<br>
-Change format of the log-files. Consider using csv. First character on each line specifying type of line, whether it's logEntry or reference or sensor definition.
-Dockerize project </br>
