namespace MySampleCosmosDb
{
    public interface IAggregateRoot
    {
        public string Id { get; }
        public string PartitionKey { get; }
    }
}
