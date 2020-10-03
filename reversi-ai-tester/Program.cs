using System;

namespace reversi_ai_tester
{
    using System.Diagnostics;
    using System.Threading;
    using IntroToGameDev.Reversi;

    class Program
    {
        private static CancellationTokenSource token = new CancellationTokenSource();

        static void Main(string[] args)
        {
            new ReversiTester().ExecuteSingleTest();
        }

        private static void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void OnExited(object? sender, EventArgs e)
        {
            token.Cancel();
        }
    }
}