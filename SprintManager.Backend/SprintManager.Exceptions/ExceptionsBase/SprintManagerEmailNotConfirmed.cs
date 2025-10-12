namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerEmailNotConfirmed : Exception
    {
        public SprintManagerEmailNotConfirmed()
        {

        }

        public SprintManagerEmailNotConfirmed(string message)
            : base(message)
        {
        }

        public SprintManagerEmailNotConfirmed(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}