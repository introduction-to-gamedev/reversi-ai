namespace IntroToGameDev.Reversi
{
    public class SingleTestResult
    {
        public TestResultType Type { get; }
        
        public string Error { get; }

        public bool IsCompletedSuccessfully => Type == TestResultType.Win || Type == TestResultType.Loss;

        public SingleTestResult(TestResultType type)
        {
            Type = type;
        }

        public SingleTestResult(TestResultType type, string error)
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
        Win, Loss, TechnicalLoss, InternalError
    }
}