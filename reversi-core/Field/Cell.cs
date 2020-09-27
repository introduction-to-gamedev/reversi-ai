namespace IntroToGameDev.Reversi
{
    using System;

    public interface ICell
    {
        
        
        bool HasPiece { get; }
        
        Piece Piece { get; }

        void Place(Piece piece);
    }
    
    public class Cell : ICell
    {
        public bool HasPiece => Piece != null;
        
        public Piece Piece { get; private set; }
        
        public void Place(Piece piece)
        {
            if (HasPiece)
            {
                throw new Exception("Cell already has a piece");
            }

            Piece = piece;
        }
    }
    
}