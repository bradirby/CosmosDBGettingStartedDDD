using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CosmosDbRepository
{
    public abstract class BaseCosmosDB 
    {
        protected Database Database;
        protected List<Container> Containers = new List<Container>();
        protected ICosmosDbDescriptor Descriptor { get; set; }
        private ICosmosDbRepositoryLogger Logger { get; set; }

        public BaseCosmosDB(ICosmosDbRepositoryLogger log = null)
        {
            Logger = log;
        }

        protected async Task InitAsync(string endPointUri, string primaryKey, ICosmosDbDescriptor desc)
        {
            Descriptor = desc;
            var cosmosClient = await CreateClient(endPointUri, primaryKey, desc.ClientOptions);
            Database = await CreateDatabase(cosmosClient);
            foreach (var cosmosDbContainerDescriptor in Descriptor.Containers)
            {
                Containers.Add(await CreateContainer(cosmosDbContainerDescriptor));    
            }
            
        }

        private Task<CosmosClient> CreateClient(string endPointUri, string primaryKey, CosmosClientOptions options)
        {
            try
            {
                var cosmosClient = new CosmosClient(endPointUri, primaryKey, options);
                return Task.FromResult( cosmosClient);
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private async Task<Database> CreateDatabase(CosmosClient client)
        {
            try
            {
                var dbResponse = await client.CreateDatabaseIfNotExistsAsync(Descriptor.DatabaseId);
                return dbResponse.Database;
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private async Task<Container> CreateContainer(ICosmosDbContainerDescriptor desc)
        {
            try
            {
                var containerResponse = await Database.CreateContainerIfNotExistsAsync(desc.ContainerId, desc.PartitionKeyPath, desc.ContainerThroughput);
                var container = containerResponse.Container;
                if (desc.ContainerThroughput != 400) await ReplaceThroughputAsync(container, desc.ContainerThroughput);
                return container;
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task DeleteDatabaseAsync()
        {
            try
            {
                var databaseResourceResponse = await Database.DeleteAsync();
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }


        public async Task ReplaceThroughputAsync(Container c, int i)
        {
            try
            {
                await c.ReplaceThroughputAsync(i);
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}
