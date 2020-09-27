namespace IntroToGameDev.Reversi
{
    public class Piece
    {
        public Color Color { get; private set; }

        public Piece(Color color)
        {
            Color = color;
        }

        public void SwitchColor()
        {
            Color = Color.Opposite();
        }
    }

    public static class ColorExtensions
    {
        public static Color Opposite(this Color color)
        {
            return color == Color.Black ? Color.White : Color.Black;
        }
    }
}