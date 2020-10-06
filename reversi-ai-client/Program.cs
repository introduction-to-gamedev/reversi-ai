namespace IntroToGameDev.Reversi.Client
{
    using System;
    using System.Linq;
    using Moves;

    class Program
    {
        static Random random = new Random();
        
        static void Main(string[] args)
        {
            var blackHole = ReadCoordinates();
            var myColor = ReadColor();
            var game = new ReversiGame(new GameFieldFactory().PrepareField(blackHole));

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
            string nextMove = null;
            while (nextMove == null)
            {
                nextMove = Console.ReadLine();    
            }
            game.MakeMove(nextMove);
        }

        private static void MakeMove(ReversiGame reversiGame, Color myColor)
        {
            var moves = new PossibleMovesFinder().GetPossibleMoves(reversiGame.Field).Where(move => move.Color == myColor).ToList();
            // Console.WriteLine($"//{string.Join(",", moves)}, {reversiGame.Field.GetCell("B4").Piece?.Color}");
            if (!moves.Any())
            {
                Console.WriteLine("pass");
                reversiGame.MakeMove("pass");
                return;
            }
          
            var move = moves[random.Next(moves.Count)];
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

            throw new ArgumentException($"{line} is not a valid color");
        }

        private static Position ReadCoordinates()
        {
            return new PositionParser().Parse(Console.ReadLine());
        }
    }
}