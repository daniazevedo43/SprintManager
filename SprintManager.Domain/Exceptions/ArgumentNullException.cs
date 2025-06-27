﻿namespace SprintManager.Domain.Exceptions
{
    public class ArgumentNullException : Exception
    {
        public ArgumentNullException() 
        { 
        
        }

        public ArgumentNullException(string message)
            : base(message)
        {
        }

        public ArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
