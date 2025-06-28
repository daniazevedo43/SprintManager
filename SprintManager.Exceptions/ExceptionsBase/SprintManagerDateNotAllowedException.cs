namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerDateNotAllowedException : Exception
    {
        public DateTime InvalidDate { get; }

        public SprintManagerDateNotAllowedException()
        {

        }

        public SprintManagerDateNotAllowedException(string message)
            : base(message)
        {
        }

        public SprintManagerDateNotAllowedException(string message, string paramName)
            : base(message)
        {
        }

        public SprintManagerDateNotAllowedException(string message, DateTime invalidDate, string paramName)
            : base(message)
        {
            InvalidDate = invalidDate;
        }
    }
}
