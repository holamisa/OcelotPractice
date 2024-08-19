namespace Middleware.Exceptions.types
{
    public class BadRequestException : Exception
    {
        public IEnumerable<string>? Errors { get; }

        public BadRequestException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors = errors;
        }

        public BadRequestException(string message) : base(message) { }
    }
}
