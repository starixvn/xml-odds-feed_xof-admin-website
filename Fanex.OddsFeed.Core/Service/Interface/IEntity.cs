namespace Fanex.OddsFeed.Core.Service.Interface
{
    public interface IEntity<TId, TName>
    {
        TId Id
        {
            get;
            set;
        }

        TName Name
        {
            get;
            set;
        }
    }
}
