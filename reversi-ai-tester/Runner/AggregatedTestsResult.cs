namespace IntroToGameDev.Reversi
{
    using System.Collections.Generic;
    using System.Linq;

    public class AggregatedTestsResult
    {
        public int TotalTests { get; set; }
        
        public int EndedWithoutErrors { get; set; }
        
        public float WinRate { get; set; }

        public override string ToString()
        {
            return $"{nameof(TotalTests)}: {TotalTests},\n {nameof(EndedWithoutErrors)}: {EndedWithoutErrors},\n {nameof(WinRate)}: {WinRate}, ";
        }
    }

    public interface IResultsAggregator
    {
        AggregatedTestsResult Aggregate(IEnumerable<SingleTestResult> results);

    }

    class ResultsAggregator : IResultsAggregator
    {
        public AggregatedTestsResult Aggregate(IEnumerable<SingleTestResult> enumerable)
        {
            var results = enumerable.ToList();
            return new AggregatedTestsResult()
            {
                TotalTests = results.Count,
                EndedWithoutErrors = results.Count(result => result.Type != TestResultType.TechnicalLoss),
                WinRate = results.Count(result => result.Type == TestResultType.Win) / (float)results.Count
            };
        }
    }
}