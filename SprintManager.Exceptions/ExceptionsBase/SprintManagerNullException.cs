namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerNullException : Exception
    {
        public SprintManagerNullException() 
        { 
        
        }

        public SprintManagerNullException(string message)
            : base(message)
        {
        }

        public SprintManagerNullException(string message, string paramName)
            : base(message)
        {
        }
    }
}
