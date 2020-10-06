namespace IntroToGameDev.Reversi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moves;
    using Utils;

    public class ReversiField
    {
        public int Width => cells.GetLength(0);

        public int Height => cells.GetLength(1);

        private readonly IPositionParser positionParser = new PositionParser();

        private readonly StreakFinder streakFinder = new StreakFinder();

        private readonly Cell[,] cells;

        public ReversiField(Cell[,] cells)
        {
            this.cells = cells;
        }

        public IEnumerable<(Position position, Cell cell)> AllCells()
        {
            for (var row = 0; row < Height; row++)
            {
                for (var column = 0; column < Width; column++)
                {
                    var cell = cells[row, column];
                    if (cell != null)
                    {
                        yield return (new Position(row, column), cell);
                    }
                }
            }
        }

        public Cell GetCell(string code)
        {
            var position = positionParser.TryParse(code);
            if (!position.HasValue)
            {
                return null;
            }

            return GetCell(position.Value);
        }

        public Cell GetCell(Position position)
        {
            if (position.Column >= Width || position.Row >= Height || position.Column < 0 || position.Row < 0)
            {
                return null;
            }

            return cells[position.Row, position.Column];
        }

        public ValueOrError<bool> MakeMove(string move, Color color)
        {
            var position = positionParser.TryParse(move);
            if (!position.HasValue)
            {
                
                return ValueOrError.FromError<bool>($"Can not find cell by code '{move}'");
            }

            return MakeMove(position.Value, color);
        }
        
        public ValueOrError<bool> MakeMove(Position move, Color color)
        {
            var cell = GetCell(move);
            if (cell != null)
            {
                cell.Place(new Piece(color));
                var streaks = streakFinder.GetStreaksFor(move, this);
                streaks
                    .Where(streak => streak.Any(c => c.Piece.Color == color))
                    .SelectMany(streak => streak.TakeWhile(c => c.Piece.Color.Opposite() == color))
                    .ToList().ForEach(c => c.Piece.SwitchColor());

                return ValueOrError.FromValue(true);
            }

            return ValueOrError.FromError<bool>($"Can not find cell with position '{move}'");
        }
    }
}