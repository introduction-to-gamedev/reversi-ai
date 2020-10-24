namespace IntroToGameDev.Reversi.Tester
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CommandLine;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Config;
    using NLog.Extensions.Logging;
    using NLog.Targets;
    using Options;
    using LogLevel = NLog.LogLevel;

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(ExecuteTests);
        }

        private static async void ExecuteTests(CommandLineOptions options)
        {
            var logger = LogManager.LogFactory.GetLogger("Logger");
            
            try
            {
                SetUpLogging(options);

                if (options.SingleRun)
                {
                    logger.Log(LogLevel.Info, "Executing single run...");
                    var result = await new SingleTestExecutor(logger).Execute(options.RunCommand);
                    logger.Log(result.IsCompletedSuccessfully ? LogLevel.Info : LogLevel.Error, $"{result.Type} {result.Error}");
                    return;
                }

                logger.Log(LogLevel.Info, "Executing full test...");

                var aggregated = new ResultsAggregator().Aggregate(await Task.WhenAll(Enumerable.Range(1, 100)
                    .Select(index =>
                    {
                        logger.Log(LogLevel.Info, $"----------------------");
                        logger.Log(LogLevel.Info, $"Executing run #{index}");
                        return new SingleTestExecutor(logger).Execute(options.RunCommand);
                    })));

                Console.WriteLine(aggregated);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                throw;
            }
        }

        private static void SetUpLogging(CommandLineOptions commandLineOptions)
        {
            var config = new LoggingConfiguration();

            if (commandLineOptions.WriteLogsToFile)
            {
                var logfile = new FileTarget("logfile") {FileName = "run_results.log"};
                config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);    
            }

            if (commandLineOptions.WriteLogsToConsole)
            {
                var logconsole = new ConsoleTarget("logconsole");
                config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            }
            
            LogManager.Configuration = config;
        }
    }
}