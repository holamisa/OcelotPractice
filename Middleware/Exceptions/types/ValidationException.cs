namespace Middleware.Exceptions.types
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
