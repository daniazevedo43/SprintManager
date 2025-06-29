namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerTooLongException : Exception
    {
        public int MaxLength { get; }
        public int ActualLength { get; }
        public string ParamName { get; }

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

        public SprintManagerTooLongException(string message, int maxLength, int actualLength)
            : base(message)
        {
            MaxLength = maxLength;
            ActualLength = actualLength;
        }

        public SprintManagerTooLongException(string message, int maxLength, int actualLength, string paramName)
            : base($"{message} (Max length '{maxLength}') (Actual length '{actualLength}') (Parameter '{paramName}')")
        {
            MaxLength = maxLength;
            ActualLength = actualLength;
            ParamName = paramName; 
        }
    }
}
