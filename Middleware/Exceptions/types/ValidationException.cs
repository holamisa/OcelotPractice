namespace Middleware.Exceptions.types
{
    public class ValidationException : Exception
    {
        public IEnumerable<string>? Errors { get; }

        public ValidationException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors = errors;
        }

        public ValidationException(string message) : base(message) { }
    }
}
