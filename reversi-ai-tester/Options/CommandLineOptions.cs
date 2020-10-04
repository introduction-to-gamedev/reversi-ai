namespace IntroToGameDev.Reversi.Tester.Options
{
    using CommandLine;

    public class CommandLineOptions
    {
        [Option("command", Required = true, HelpText = "Command line script to start your client")]
        public string RunCommand { get; set; }
        
        [Option('s', "write-logs", Required = false, HelpText = "Write logs for played runs")]
        public bool WriteLogs { get; set; }
        
        [Option('s',"single-run", Required = false, HelpText = "Execute only single run, instead of full 100 runs cycle")]
        public bool SingleRun { get; set; }
    }
}