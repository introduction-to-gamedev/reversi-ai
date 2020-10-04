namespace IntroToGameDev.Reversi
{
    public class SingleTestResult
    {
        public TestResultType Type { get; }
        
        public string Error { get; }

        public SingleTestResult(TestResultType type)
        {
            Type = type;
        }

        private SingleTestResult(TestResultType type, string error)
        {
            Type = type;
            Error = error;
        }

        public static SingleTestResult FromError(string error)
        {
            return new SingleTestResult(TestResultType.TechnicalLoss, error);
        }
    }

    public enum TestResultType
    {
        Win, Loss, TechnicalLoss
    }
}