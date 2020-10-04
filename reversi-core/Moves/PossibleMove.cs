namespace IntroToGameDev.Reversi.Moves
{
    using System;

    public class PossibleMove
    {
        public Position Position { get; set; }
        
        public Color Color { get; set; }

        public PossibleMove(Position position, Color color)
        {
            Position = position;
            Color = color;
        }

        public PossibleMove()
        {
        }

        protected bool Equals(PossibleMove other)
        {
            return Position.Equals(other.Position) && Color == other.Color;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PossibleMove) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, (int) Color);
        }

        public override string ToString()
        {
            return $"{Position.ToCode()} ({Color})";
        }
    }

    public static class PositionExtensions
    {
        public static string ToCode(this Position position)
        {
            var symbol = (char)('A' + position.Column);
            var number = (char)('1' + position.Row);

            return $"{symbol.ToString()}{number.ToString()}";
        }
    }
}