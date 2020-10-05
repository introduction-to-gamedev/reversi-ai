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
        async static Task Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(ExecuteTests);
        }

        private static async void ExecuteTests(CommandLineOptions options)
        {
            //logs are written into console only in case we're executing single run
            var debugLog = options.SingleRun || options.WriteLogs;

            SetUpLogging();
            var logger = LogManager.LogFactory.GetLogger("Logger");

            if (options.SingleRun)
            {
                logger.Log(LogLevel.Info, "Executing single run...");
                var result = await new ReversiTester(logger).ExecuteSingleTest(options.RunCommand);
                logger.Log(result.Type == TestResultType.TechnicalLoss ? LogLevel.Error : LogLevel.Info,
                    $"{result.Type} {result.Error}");
                return;
            }

            logger.Log(LogLevel.Info, "Executing full test...");

            var aggregated = new ResultsAggregator().Aggregate(await Task.WhenAll(Enumerable.Range(1, 10)
                .Select(index =>
                {
                    logger.Log(LogLevel.Info, $"----------------------");
                    logger.Log(LogLevel.Info, $"Executing run #{index}");
                    return new ReversiTester(logger).ExecuteSingleTest(options.RunCommand);
                })));

            Console.WriteLine(aggregated);
        }

        private static void SetUpLogging()
        {
            var config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new FileTarget("logfile") {FileName = "run_results.log"};
            var logconsole = new ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
        }
    }
}