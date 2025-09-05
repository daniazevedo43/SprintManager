namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerInvalidUsernameException : Exception
    {
        public SprintManagerInvalidUsernameException()
        {
        }

        public SprintManagerInvalidUsernameException(string message) : base(message) 
        { 
        }
    }
}