﻿namespace IntroToGameDev.Reversi
{
    public interface IPositionParser
    {
        Position? TryParse(string code);
    }
    
    public class PositionParser : IPositionParser
    {
        public Position? TryParse(string code)
        {
            code = code.ToLower();
            if (code.Length != 2)
            {
                return null;
            }

            var symbol = code[0];
            var number = code[1];
            
            var column = symbol - 'a';
            var row = number - '1';

            if (row >= 0 && column >= 0 && row <= 7 && column <= 7)
            {
                return new Position(row, column);
            }

            return null;
        }
    }
}