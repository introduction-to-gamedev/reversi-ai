namespace IntroToGameDev.Reversi
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Moves;

    public class ReversiTester
    {
        private readonly TaskCompletionSource<bool> processExitCts = new TaskCompletionSource<bool>();
        private readonly TaskCompletionSource<bool> gameOverCts = new TaskCompletionSource<bool>();

        public void ExecuteSingleTest()
        {
            var process =
                Process.Start(
                    "E:\\Projects\\IntroToGamedev\\reversi\\reversi-ai-client\\bin\\Debug\\netcoreapp3.1\\reversi-ai-client.exe");
            if (process == null)
            {
                return;
            }


            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.EnableRaisingEvents = true;

            process.Exited += OnExited;
            process.ErrorDataReceived += OnErrorDataReceived;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            Play(process);
            var result = Task.WaitAny(processExitCts.Task, gameOverCts.Task);
        }

        private async Task Play(Process process)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            var output = process.StandardOutput;
            var input = process.StandardInput;
            
            var game = new ReversiGame(new GameFieldFactory().PrepareField());
            
            await input.WriteLineAsync("A1");
            await input.WriteLineAsync("black");

            while (!game.IsOver)
            {
                var line = await output.ReadLineAsync();
                game.MakeMove(line);
                
                var moves = new PossibleMovesFinder().GetPossibleMoves(game.Field).Where(move => move.Color == Color.White);
                var move = moves.First().Position.ToCode();
                var result = game.MakeMove(move);

                await input.WriteLineAsync(move);
            }

            
            
            gameOverCts.SetResult(true);
            process.Kill();
        }

        private static void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnExited(object? sender, EventArgs e)
        {
            processExitCts.SetException(new Exception());
        }
    }
}