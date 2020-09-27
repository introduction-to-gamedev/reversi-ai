namespace IntroToGameDev.Reversi
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class PositionParserTests
    {
        private readonly IPositionParser parser = new PositionParser();

        [TestCaseSource(nameof(GetParsePairs))]
        public void ParserShouldParsePosition(string input, Position? result)
        {
            Assert.That(parser.TryParse(input), Is.EqualTo(result));
        }

        private static object[] GetParsePairs()
        {
            return new Object[]
            {
                new object[] {"A1", new Position(0, 0)},
                new object[] {"H8", new Position(7, 7)},
                new object[] {"F4", new Position(3, 5)},
                new object[] {"I5", null},
                new object[] {"H9", null},
                new object[] {"unknown", null},
            };
        }
    }
}