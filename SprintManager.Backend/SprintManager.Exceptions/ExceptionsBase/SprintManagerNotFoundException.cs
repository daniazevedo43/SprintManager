namespace SprintManager.Exceptions.ExceptionsBase
{
    public class SprintManagerNotFoundException : Exception
    {
        public string? ParamName { get; }

        public SprintManagerNotFoundException() { }

        public SprintManagerNotFoundException(string message) : base(message) { }

        public SprintManagerNotFoundException(string message, string paramName)
            : base(message)
        {
            ParamName = paramName;
        }
    }
}
