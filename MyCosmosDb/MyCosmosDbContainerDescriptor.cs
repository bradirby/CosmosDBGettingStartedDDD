using CosmosDbRepository;

namespace MySampleCosmosDb
{
    public class MyCosmosDbContainerDescriptor : ICosmosDbContainerDescriptor
    {
        public int ContainerThroughput { get; set; }
        public string ContainerId { get; set; }
        public string PartitionKeyPath { get; set; }

        public MyCosmosDbContainerDescriptor()
        {
            ContainerId = "items";
            ContainerThroughput = 500;
            PartitionKeyPath = "LastName";
        }

    }
}