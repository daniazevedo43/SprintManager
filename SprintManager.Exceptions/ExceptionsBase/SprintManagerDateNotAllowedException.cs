namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerDateNotAllowedException : Exception
    {
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
    }
}