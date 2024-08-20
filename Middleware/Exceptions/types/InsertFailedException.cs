namespace Middleware.Exceptions.types
{
    public class InsertFailedException : Exception
    {
        public InsertFailedException(string message) : base(message) { }
    }
}
