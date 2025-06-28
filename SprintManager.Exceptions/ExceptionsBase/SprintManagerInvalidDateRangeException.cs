namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerInvalidDateRangeException : Exception
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

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

        public SprintManagerInvalidDateRangeException(string message, DateTime startDate, DateTime endDate, string paramName)
            : base(message)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
