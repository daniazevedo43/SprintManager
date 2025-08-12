namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerFileNotAllowedException : Exception
    {
        public SprintManagerFileNotAllowedException() : base() 
        { 
        }

        public SprintManagerFileNotAllowedException(string message) : base(message)
        { 
        }

        public SprintManagerFileNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}