namespace BoundedContext
{
    public interface IAggregateRoot
    {
        public string Id { get; }
        public string PartitionKey { get; }
    }
}
