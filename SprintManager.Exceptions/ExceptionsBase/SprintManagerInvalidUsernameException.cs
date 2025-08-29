namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerInvalidUsernameException : Exception
    {
        public SprintManagerInvalidUsernameException() : base()
        {
        }

        public SprintManagerInvalidUsernameException(string message) : base(message) 
        { 
        }
    }
}