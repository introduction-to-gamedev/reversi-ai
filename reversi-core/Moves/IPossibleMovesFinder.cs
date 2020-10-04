namespace IntroToGameDev.Reversi.Moves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPossibleMovesFinder
    {
        IEnumerable<PossibleMove> GetPossibleMoves(ReversiField field);
    }

    public class PossibleMovesFinder : IPossibleMovesFinder
    {
        private readonly StreakFinder streakFinder = new StreakFinder();
        
        public IEnumerable<PossibleMove> GetPossibleMoves(ReversiField field)
        {
            foreach (var (position, cell) in field.AllCells())
            {
                if (cell.HasPiece)
                {
                    continue;
                }
                var streaks = streakFinder.GetStreaksFor(position, field);
                foreach (var streak in streaks)
                {
                    var firstPoint = streak[0];
                    var firstPieceOppositeColor = firstPoint.Piece.Color.Opposite();
                    if (streak.Count > 1 &&
                        streak.Any(c => c.HasPiece && c.Piece.Color == firstPieceOppositeColor))
                    {
                        yield return new PossibleMove(position, firstPieceOppositeColor);
                    }
                }
            }
        }
    }

    public class StreakFinder
    {
        public IEnumerable<List<Cell>> GetStreaksFor(Position origin, ReversiField field)
        {
            var directionOffsets = new List<Position>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        directionOffsets.Add(new Position(x, y));
                    }
                }
            }
            
            foreach (var offset in directionOffsets)
            {
                var streak = new List<Cell>();
                Cell next;
                var nextPosition = origin;
                do
                {
                    nextPosition += offset;
                    next = field.GetCell(nextPosition);
                    if (next != null && next.HasPiece)
                    {
                        streak.Add(next);
                    }
                } while (next != null && next.HasPiece);

                if (streak.Any())
                {
                    yield return streak;
                }
            }
        }
    }
}