namespace IntroToGameDev.Reversi
{
    using System;

    public struct Position
    {
        public int Row { get; }
        
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(Position other)
        {
            return Row == other.Row && Column == other.Column;
        }
        
        public static Position operator +(Position left, Position right)
        {
            return new Position(left.Row + right.Row, left.Column + right.Column);
        }
        
        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        public static implicit operator Position((int row, int column) tuple)
        {
            var (row, column) = tuple;
            return new Position(row, column);
        } 

        public override bool Equals(object obj)
        {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public override string ToString()
        {
            return $"[{Row}, {Column}]";
        }
    }
}