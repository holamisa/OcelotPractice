namespace Middleware.Exceptions.types
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
