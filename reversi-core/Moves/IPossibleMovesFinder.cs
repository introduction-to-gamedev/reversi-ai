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
        public IEnumerable<PossibleMove> GetPossibleMoves(ReversiField field)
        {
            var directionOffsets = new List<Position>()
            {
                (1, 0), (-1, 0), (0, 1), (0, -1), (1, 1), (1, -1), (-1, 1), (-1, -1)
            };
            foreach (var (position, cell) in field.AllCells())
            {
                if (cell.HasPiece)
                {
                    continue;
                }

                foreach (var offset in directionOffsets)
                {
                    var streak = new List<Cell>();
                    Cell next;
                    var nextPosition = position;
                    do
                    {
                        nextPosition += offset;
                        next = field.GetCell(nextPosition);
                        if (next != null && next.HasPiece)
                        {
                            streak.Add(next);
                            continue;
                        }
                        if (streak.Any(c => c.Piece.Color == Color.White) &&
                            streak.Any(c => c.Piece.Color == Color.Black))
                        {
                            yield return new PossibleMove(position, streak.First().Piece.Color.Opposite());
                        }
                    } while (next != null && next.HasPiece);
                }
            }
        }
    }

    public class StreakFinder
    {
        public IEnumerable<List<Cell>> GetStreaksFor(Position origin, ReversiField field)
        {
            var directionOffsets = new List<Position>()
            {
                (1, 0), (-1, 0), (0, 1), (0, -1), (1, 1), (1, -1), (-1, 1), (-1, -1)
            };
            
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

                yield return streak;
            }
        }
    }
}