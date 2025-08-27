namespace BoardGamePlayer.Infrastructure.Exceptions
{
    [Serializable]
    public class QueryWriteException : Exception
    {
        public QueryWriteException() { }
        public QueryWriteException(string? message) : base(message) { }
        public QueryWriteException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}