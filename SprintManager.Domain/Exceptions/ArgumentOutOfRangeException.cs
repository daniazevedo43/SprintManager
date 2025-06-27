namespace SprintManager.Domain.Exceptions
{
    public class ArgumentOutOfRangeException : Exception
    {
        public int? MaxLimit { get; }
        public int? MinLimit { get; } 
        public int ActualLength { get; }

        public ArgumentOutOfRangeException()
        {
        }

        public ArgumentOutOfRangeException(string message)
            : base(message)
        {
        }

        public ArgumentOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArgumentOutOfRangeException(string message, int? minLimit, int? maxLimit, int actualLength)
            : base(message)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
            ActualLength = actualLength;
        }

        public ArgumentOutOfRangeException(string message, int? minLimit, int? maxLimit, int actualLength, Exception innerException)
            : base(message, innerException)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
            ActualLength = actualLength;
        }
    }
}
