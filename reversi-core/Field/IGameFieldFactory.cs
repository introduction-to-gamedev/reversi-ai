namespace IntroToGameDev.Reversi
{
    public interface IGameFieldFactory
    {
        ReversiField PrepareField();
    }

    public class GameFieldFactory : IGameFieldFactory
    {
        public ReversiField PrepareField()
        {
            var size = 8;
            var cells = new Cell[size,size];
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    cells[row, column] = new Cell();  
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