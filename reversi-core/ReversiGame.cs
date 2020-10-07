namespace IntroToGameDev.Reversi
{
    using System.Linq;
    using Moves;
    using Utils;

    public class ReversiGame
    {
        public ReversiField Field { get; private set; }

        public bool IsOver => !possibleMovesFinder.GetPossibleMoves(Field).Any();

     

        public Color CurrentColor { get; private set; } = Color.Black;
        
        private readonly IPossibleMovesFinder possibleMovesFinder = new PossibleMovesFinder();
        
        private readonly IPositionParser parser = new PositionParser();

        public ReversiGame(ReversiField field)
        {
            Field = field;
        }
        
        public int GetScoreFor(Color color)
        {
            return Field.AllCells().Select(tuple => tuple.Item2)
                .Count(cell => cell.HasPiece && cell.Piece.Color == color);
        }

        public ValueOrError<bool> MakeMove(string move)
        {
            if (move == "pass")
            {
                CurrentColor = CurrentColor.Opposite();
                return ValueOrError.FromValue<bool>(true);
            }
            
            var position = parser.TryParse(move);
            if (!position.HasValue)
            {
                return ValueOrError.FromError<bool>($"Wrong code: {move}");
            }

            if (possibleMovesFinder.GetPossibleMoves(Field).Any(possibleMove =>
                possibleMove.Color == CurrentColor && possibleMove.Position == position.Value))
            {
                var result = Field.MakeMove(move, CurrentColor);
                if (result.HasValue)
                {
                    CurrentColor = CurrentColor.Opposite();
                }

                return result;    
            }
            
            return ValueOrError.FromError<bool>($"Move {move} is impossible for {CurrentColor}");
        }
        
    }
}