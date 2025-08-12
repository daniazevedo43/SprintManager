namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerInvalidDateRangeException : Exception
    {
        public SprintManagerInvalidDateRangeException()
        {

        }

        public SprintManagerInvalidDateRangeException(string message)
            : base(message)
        {
        }

        public SprintManagerInvalidDateRangeException(string message, string paramName)
            : base(message)
        {
        }
    }
}