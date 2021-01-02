namespace CosmosDbRepository
{
    public interface ICosmosDbContainerDescriptor
    {
        int ContainerThroughput { get;  }
        string ContainerId { get; }
        string PartitionKeyPath { get; }

    }

}
