using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerTooShortException : Exception
    {
        public int? MinLimit { get; }
        public int ActualLength { get; }

        public SprintManagerTooShortException()
        {
        }

        public SprintManagerTooShortException(string message)
            : base(message)
        {
        }

        public SprintManagerTooShortException(string message, string paramName)
            : base(message)
        {
        }

        public SprintManagerTooShortException(string message, int minLimit, int actualLength)
            : base(message)
        {
            MinLimit = minLimit;
            ActualLength = actualLength;
        }

        public SprintManagerTooShortException(string message, int minLimit, int actualLength, string paramName)
            : base(message)
        {
            MinLimit = minLimit;
            ActualLength = actualLength;
        }
    }
}
