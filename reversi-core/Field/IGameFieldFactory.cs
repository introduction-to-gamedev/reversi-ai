namespace IntroToGameDev.Reversi
{
    public interface IGameFieldFactory
    {
        ReversiField PrepareField(Position? blackHole = null);
    }

    public class GameFieldFactory : IGameFieldFactory
    {
        public ReversiField PrepareField(Position? blackHole = null)
        {
            var size = 8;
            var cells = new Cell[size,size];
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    var position = new Position(row, column);
                    if (position != blackHole)
                    {
                        cells[row, column] = new Cell();
                    }  
                }
            }
            
            var field = new ReversiField(cells);
            field.GetCell("D4").Place(new Piece(Color.White));
            field.GetCell("E5").Place(new Piece(Color.White));
            field.GetCell("D5").Place(new Piece(Color.Black));
            field.GetCell("E4").Place(new Piece(Color.Black));

            return field;
        }
    }
}