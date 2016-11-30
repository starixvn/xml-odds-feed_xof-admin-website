namespace Fanex.OddsFeed.Common.DTO
{
    public class QueryResult<T>
    {
        public Error Error { get; set; }

        public T Data { get; set; }
    }
}
