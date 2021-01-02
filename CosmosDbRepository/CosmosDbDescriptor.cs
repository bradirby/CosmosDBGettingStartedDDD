using System.Collections.Generic;
using Microsoft.Azure.Cosmos;

namespace CosmosDbRepository
{
    
    public interface ICosmosDbDescriptor
    {
        string DatabaseId { get;  }
        CosmosClientOptions ClientOptions { get; }
        List<ICosmosDbContainerDescriptor> Containers { get; }
    }
}