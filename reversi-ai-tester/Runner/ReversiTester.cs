namespace IntroToGameDev.Reversi
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Moves;
    using NLog;
    using Utils;
    using ILogger = NLog.ILogger;
    using LogLevel = NLog.LogLevel;

    public class ReversiTester
    {
        private readonly TaskCompletionSource<SingleTestResult> processExitCts =
            new TaskCompletionSource<SingleTestResult>();
        
        private readonly StringBuilder errorsBuilder = new StringBuilder();

        private readonly ILogger logger;

        public ReversiTester(Logger logger)
        {
            this.logger = logger;
        }

        public async Task<SingleTestResult> ExecuteSingleTest(string command)
        {
            var process =
                Process.Start(command);
            if (process == null)
            {
                return SingleTestResult.FromError($"Can not start process, please check argument:\n {command}");
            }

            

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.EnableRaisingEvents = true;

            process.Exited += OnProcessOnExited;
            process.ErrorDataReceived +=  (sender, args) => errorsBuilder.Append(args.Data);
            process.StartInfo.UseShellExecute = false;

            process.Start();
            process.BeginErrorReadLine();


            var playTask = Play(process);

            process.WaitForExit();
            
            if (errorsBuilder.Length > 0)
            {
                return SingleTestResult.FromError(errorsBuilder.ToString());
            }

            return playTask.Result;
        }

        private void OnProcessOnExited(object? sender, EventArgs args)
        {
            errorsBuilder.Append("Error: provided program terminates unexpectedly");
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }

        private async Task<SingleTestResult> Play(Process process)
        {
            try
            {
                var output = process.StandardOutput;
                var input = process.StandardInput;

                var game = new ReversiGame(new GameFieldFactory().PrepareField());

                await input.WriteLineAsync("A5");
                await input.WriteLineAsync("black");
                while (!game.IsOver)
                {
                    var moves1 = new PossibleMovesFinder().GetPossibleMoves(game.Field)
                        .Where(move => move.Color == Color.Black).ToList();
                    var command = FetchNextCommand(output);
                    if (!command.HasValue)
                    {
                        return SingleTestResult.FromError(command.Error);
                    }

                    var line = command.Value;
                    if (line == null)
                    {
                        return SingleTestResult.FromError("Error: can not fetch next move");
                    }

                    if (line.StartsWith("//"))
                    {
                        Console.WriteLine(line);
                        line = await output.ReadLineAsync();
                    }

                    logger.Log(LogLevel.Info, $"<- {line}");

                    if (moves1.Any() && line == "pass")
                    {
                        return SingleTestResult.FromError("Error: can not pass when possible moves are available");
                    }

                    game.MakeMove(line);

                    var moves = new PossibleMovesFinder().GetPossibleMoves(game.Field)
                        .Where(move => move.Color == Color.White);
                    var move = moves.First().Position.ToCode();
                    var result = game.MakeMove(move);

                    logger.Log(LogLevel.Info, $"-> {move}");
                    await input.WriteLineAsync(move);
                }

                Console.WriteLine($"Black {game.GetScoreFor(Color.Black)} : {game.GetScoreFor(Color.White)} White");

                process.Exited -= OnProcessOnExited;
                process.Kill();

                return new SingleTestResult(game.CurrentWinner == Color.Black
                    ? TestResultType.Win
                    : TestResultType.Loss);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private ValueOrError<string> FetchNextCommand(StreamReader sr)
        {
            var delayTask = Task.Delay(TimeSpan.FromSeconds(1));
            string nextCommand = null;
            while (nextCommand == null && !delayTask.IsCompleted)
            {
                nextCommand = sr.ReadLine();
            }

            if (nextCommand != null)
            {
                return ValueOrError.FromValue<string>(nextCommand);
            }
            
            return ValueOrError.FromError<string>("Error: timeout of reading next command reached");

        }


    }
}