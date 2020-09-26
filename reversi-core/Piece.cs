namespace IntroToGameDev.Reversi
{
    public class Piece
    {
        public Color Color { get; private set; }

        public Piece(Color color)
        {
            Color = color;
        }
    }
}