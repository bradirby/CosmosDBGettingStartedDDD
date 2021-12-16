using System.Collections.Generic;
using CosmosDbRepository;
using Microsoft.Azure.Cosmos;

namespace MySampleCosmosDb
{
    public class MyCosmosDbDescriptor : ICosmosDbDescriptor
    {

        public MyCosmosDbDescriptor()
        {
            DatabaseId = "MyDB";

            ClientOptions.ApplicationName = "MyAppName";

            Containers.Add(new MyCosmosDbContainerDescriptor());
        }

        public string DatabaseId { get; }
        public CosmosClientOptions ClientOptions { get; } = new CosmosClientOptions();
        public List<ICosmosDbContainerDescriptor> Containers { get; } = new List<ICosmosDbContainerDescriptor>();
    }
}
