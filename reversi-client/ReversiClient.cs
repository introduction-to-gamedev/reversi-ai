using System;

namespace reversi_client
{
    using System.Collections.Generic;
    using System.Linq;
    using IntroToGameDev.Reversi;
    using IntroToGameDev.Reversi.Moves;
    using IntroToGameDev.Reversi.Utils;

    class ReversiClient
    {
        static void Main(string[] args)
        {
            var game = new ReversiGame(new GameFieldFactory().PrepareField());
            var movesFinder = new PossibleMovesFinder();
            DrawField(game.Field, game.CurrentColor, movesFinder.GetPossibleMoves(game.Field).ToList());

            while (!game.IsOver)
            {
                var move = ReadMove();
                ValueOrError<bool> result;
                if (move == "rnd" || move == "r")
                {
                    result = game.MakeMove(movesFinder.GetPossibleMoves(game.Field)
                        .First(possibleMove => possibleMove.Color == game.CurrentColor).Position.ToCode());
                }
                else
                {
                    result = game.MakeMove(move);
                }


                if (result.HasValue)
                {
                    DrawField(game.Field, game.CurrentColor, movesFinder.GetPossibleMoves(game.Field).ToList());
                }
                else
                {
                    Console.WriteLine($"Error occured during move: {result.Error}");
                }
            }
        }

        static string ReadMove()
        {
            return Console.ReadLine();
        }

        static void DrawField(ReversiField field, Color currentMove, List<PossibleMove> possibleMoves)
        {
            var delimiter = "  ";
            Console.Write($"{delimiter} ");
            for (char letter = 'A'; letter <= 'H'; letter++)
            {
                Console.Write($"{letter}{delimiter}");
            }

            for (var row = 0; row <= 7; row++)
            {
                Console.WriteLine();
                Console.Write($"{(row + 1)}  ");
                for (var column = 0; column <= 7; column++)
                {
                    var position = new Position(row, column);
                    var cell = field.GetCell(position);
                    DrawCell(cell, position);
                }
            }

            void DrawCell(Cell cell, Position position)
            {
                string code;
                if (cell.HasPiece)
                {
                    code = cell.Piece.Color == Color.Black ? "B" : "W";
                }
                else
                {
                    code = possibleMoves.Any(move => move.Position.Equals(position) && move.Color == currentMove)
                        ? "+"
                        : ".";
                }

                Console.Write($"{code}  ");
            }

            Console.WriteLine();
        }
    }
}