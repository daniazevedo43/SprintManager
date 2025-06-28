namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerTooLongException : Exception
    {
        public int MaxLimit { get; }
        public int ActualLength { get; }

        public SprintManagerTooLongException()
        {
        }

        public SprintManagerTooLongException(string message)
            : base(message)
        {
        }

        public SprintManagerTooLongException(string message, string paramName)
            : base(message)
        {
        }

        public SprintManagerTooLongException(string message, int maxLimit, int actualLength)
            : base(message)
        {
            MaxLimit = maxLimit;
            ActualLength = actualLength;
        }

        public SprintManagerTooLongException(string message, int maxLimit, int actualLength, string paramName)
            : base(message)
        {
            MaxLimit = maxLimit;
            ActualLength = actualLength;
        }
    }
}
