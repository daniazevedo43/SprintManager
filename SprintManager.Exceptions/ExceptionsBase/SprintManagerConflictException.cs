namespace SprintManager.Application.Exceptions
{
    public class SprintManagerConflictException : Exception
    {
        public SprintManagerConflictException() : base() 
        { 
        }

        public SprintManagerConflictException(string message) : base(message) 
        { 
        }

        public SprintManagerConflictException(string message, Exception innerException) : base(message, innerException) 
        {
        }
    }
}
