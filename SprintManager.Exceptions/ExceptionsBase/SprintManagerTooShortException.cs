namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerTooShortException : Exception
    {
        public int MinLength { get; }
        public int ActualLength { get; }
        public string ParamName { get; }

        public SprintManagerTooShortException()
        {
        }

        public SprintManagerTooShortException(string message)
            : base(message)
        {
        }

        public SprintManagerTooShortException(string message, string paramName)
            : base(message)
        {
        }

        public SprintManagerTooShortException(string message, int minLimit, int actualLength)
            : base(message)
        {
            MinLength = minLimit;
            ActualLength = actualLength;
        }

        public SprintManagerTooShortException(string message, int minLength, int actualLength, string paramName)
            : base($"{message} (Min length '{minLength}') (Actual length '{actualLength}') (Parameter '{paramName}')")
        {
            MinLength = minLength;
            ActualLength = actualLength;
            ParamName = paramName;
        }
    }
}
