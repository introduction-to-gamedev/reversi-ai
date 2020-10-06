namespace IntroToGameDev.Reversi
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Moves;
    using NLog;
    using Utils;

    public class SingleTestExecutor
    {
        private readonly TaskCompletionSource<SingleTestResult> processExitCts =
            new TaskCompletionSource<SingleTestResult>();
        
        private readonly StringBuilder errorsBuilder = new StringBuilder();

        private readonly ILogger logger;

        public SingleTestExecutor(Logger logger)
        {
            this.logger = logger;
        }

        public async Task<SingleTestResult> Execute(string command)
        {
            var process = Process.Start(command);
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

        private async Task<SingleTestResult> Play(Process process)
        {
            try
            {
                var random = new Random();
                var output = process.StandardOutput;
                var input = process.StandardInput;

                var game = new ReversiGame(new GameFieldFactory().PrepareField());

                await input.WriteLineAsync("A5");

                var playersColor = random.NextDouble() > .5? Color.Black : Color.White;
                logger.Log(LogLevel.Info, $"Chosen color for player: {playersColor}");
                await input.WriteLineAsync(playersColor.ToString().ToLower());

                if (playersColor == Color.White)
                {
                    await PerformMove();
                }

                while (!game.IsOver)
                {
                    var possibleMoves = new PossibleMovesFinder().GetPossibleMoves(game.Field)
                        .Where(move => move.Color == playersColor).ToList();
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

                    logger.Log(LogLevel.Info, $"<- {line}");
                    if (possibleMoves.Any() && line == "pass")
                    {
                        return SingleTestResult.FromError("Error: can not pass when possible moves are available");
                    }

                    game.MakeMove(line);

                    await PerformMove();
                }

                Console.WriteLine($"Black {game.GetScoreFor(Color.Black)} : {game.GetScoreFor(Color.White)} White");

               

                return new SingleTestResult(game.CurrentWinner == playersColor
                    ? TestResultType.Win
                    : TestResultType.Loss);

                Task PerformMove()
                {
                    var moves = new PossibleMovesFinder().GetPossibleMoves(game.Field)
                        .Where(move => move.Color == playersColor.Opposite()).ToList();
                    var move = moves.Any()? moves[random.Next(moves.Count)].Position.ToCode() : "pass";
                    game.MakeMove(move);

                    logger.Log(LogLevel.Info, $"-> {move}");
                    return input.WriteLineAsync(move);
                }
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, $"Internal error occured: {e.StackTrace}");
                return new SingleTestResult(TestResultType.InternalError, e.Message); 
            }
            finally
            {
                process.Exited -= OnProcessOnExited;
                process.Kill();
            }
           
        }

        private ValueOrError<string> FetchNextCommand(StreamReader sr)
        {
            var delayTask = Task.Delay(TimeSpan.FromSeconds(1));
            string nextCommand = null;
            while (IsNullOrComment(nextCommand) && !delayTask.IsCompleted)
            {
                nextCommand = sr.ReadLine();
                if (nextCommand != null && nextCommand.StartsWith("//"))
                {
                    logger.Log(LogLevel.Info, nextCommand);
                }
            }

            if (nextCommand != null)
            {
                return ValueOrError.FromValue(nextCommand);
            }
            
            return ValueOrError.FromError<string>("Error: timeout of reading next command reached");

            bool IsNullOrComment(string value)
            {
                return value == null || value.StartsWith("//");
            }
        }


    }
}