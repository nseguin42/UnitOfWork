namespace UnitOfWork
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
