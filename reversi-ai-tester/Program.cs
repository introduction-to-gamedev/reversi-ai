namespace IntroToGameDev.Reversi.Tester
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommandLine;
    using Options;

    class Program
    {
        async static Task Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(ExecuteTests);
        }

        private static async void ExecuteTests(CommandLineOptions options)
        {
            // if (options.SingleRun)
            // {
            //     Console.WriteLine("Executing single run...");
            //     var result = await new ReversiTester().ExecuteSingleTest(options.RunCommand);
            //     Console.WriteLine($"{result.Type} {result.Error}");
            //     return;
            // }

            Console.WriteLine("Executing full test...");

            var aggregated = new ResultsAggregator().Aggregate(await Task.WhenAll(Enumerable.Range(1, 10)
                .Select(index => new ReversiTester().ExecuteSingleTest(options.RunCommand))));
            
            Console.WriteLine(aggregated);
        }
    }
}