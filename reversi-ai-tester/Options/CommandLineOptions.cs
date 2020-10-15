namespace IntroToGameDev.Reversi.Tester.Options
{
    using CommandLine;

    public class CommandLineOptions
    {
        [Option("command", Required = true, HelpText = "Command line script to start your client")]
        public string RunCommand { get; set; }
        
        [Option('c', "console-logs", Required = false, Default = true, HelpText = "Write logs to console for played runs")]
        public bool WriteLogsToConsole { get; set; }
        
        [Option('f', "file-logs", Required = false, HelpText = "Write logs to file for played runs")]
        public bool WriteLogsToFile { get; set; }
        
        [Option('s',"single-run", Required = false, HelpText = "Execute only single run, instead of full 100 runs cycle")]
        public bool SingleRun { get; set; }
    }
}