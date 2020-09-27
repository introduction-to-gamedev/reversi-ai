namespace IntroToGameDev.Reversi
{
    using NUnit.Framework;

    [TestFixture]
    public class GameFieldTests
    {
        [Test]
        public void FactoryShouldCreateCorrectField()
        {
            var field = new GameFieldFactory().PrepareField();
            
            Assert.That(field.Height, Is.EqualTo(8));
            Assert.That(field.Width, Is.EqualTo(8));
            
            Assert.That(field.GetCell("D4").Piece.Color, Is.EqualTo(Color.White));
            Assert.That(field.GetCell("D5").Piece.Color, Is.EqualTo(Color.Black));
            Assert.That(field.GetCell("E4").Piece.Color, Is.EqualTo(Color.Black));
            Assert.That(field.GetCell("E5").Piece.Color, Is.EqualTo(Color.White));
            
            Assert.That(field.GetCell("E6").HasPiece, Is.False);
        }
    }
    
    
}