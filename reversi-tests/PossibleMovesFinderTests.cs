namespace IntroToGameDev.Reversi
{
    using System.Collections.Generic;
    using Moves;
    using NUnit.Framework;

    [TestFixture]
    public class PossibleMovesFinderTests
    {
        [Test]
        public void FinderShouldFindMovesAtStartingPosition()
        {
            var moves = new PossibleMovesFinder().GetPossibleMoves(new GameFieldFactory().PrepareField());
            var parser = new PositionParser();
            
            var expectedMoves = new List<PossibleMove>()
            {
                new PossibleMove(){Color = Color.White, Position = parser.Parse("E3")},
                new PossibleMove(){Color = Color.White, Position = parser.Parse("F4")},
                new PossibleMove(){Color = Color.White, Position = parser.Parse("C5")},
                new PossibleMove(){Color = Color.White, Position = parser.Parse("D6")},
                
                new PossibleMove(){Color = Color.Black, Position = parser.Parse("C4")},
                new PossibleMove(){Color = Color.Black, Position = parser.Parse("D3")},
                new PossibleMove(){Color = Color.Black, Position = parser.Parse("F5")},
                new PossibleMove(){Color = Color.Black, Position = parser.Parse("E6")},
            };
            
            CollectionAssert.AreEquivalent(moves, expectedMoves);
        }
    }
}