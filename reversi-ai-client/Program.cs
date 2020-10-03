using System;

namespace reversi_ai_client
{
    using System.Linq;
    using IntroToGameDev.Reversi;
    using IntroToGameDev.Reversi.Moves;

    class Program
    {
        static void Main(string[] args)
        {
            var blackHole = ReadCoordinates();

            var myColor = ReadColor();
            
            var game = new ReversiGame(new GameFieldFactory().PrepareField());

            if (myColor == Color.Black)
            {
                MakeMove(game, myColor);
            }
            
            while (!game.IsOver)
            {
                GetOpponentsMove(game);

                if (game.IsOver)
                {
                    break;
                }
                
                MakeMove(game, myColor);
            }
        }

        private static void GetOpponentsMove(ReversiGame game)
        {
            game.MakeMove(Console.ReadLine());
        }

        private static void MakeMove(ReversiGame reversiGame, Color myColor)
        {
            var moves = new PossibleMovesFinder().GetPossibleMoves(reversiGame.Field).Where(move => move.Color == myColor).ToList();
            if (!moves.Any())
            {
                Console.WriteLine("pass");
                return;
            }
            
            var move = moves.First();
            var code = move.Position.ToCode();
            reversiGame.MakeMove(code);
            Console.WriteLine(code);
        }

        private static Color ReadColor()
        {
            var line = Console.ReadLine();
            if (line == "white")
            {
                return Color.White;
            }

            if (line == "black")
            {
                return Color.Black;
            }

            throw new ArgumentException();
        }

        private static Position ReadCoordinates()
        {
            return new PositionParser().Parse(Console.ReadLine());
        }
    }
}