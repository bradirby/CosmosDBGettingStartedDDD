using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CosmosDbRepository
{

    public abstract class BaseCosmosDBRepository<T> where T:class, new()
    {
        protected Container container { get; set; }
        protected ICosmosDbRepositoryLogger Logger;

        protected BaseCosmosDBRepository(Container cosmosContainer, ICosmosDbRepositoryLogger log)
        {
            Logger = log;
            container = cosmosContainer ?? throw new ArgumentNullException(nameof(cosmosContainer));
        }


        protected async Task SaveAsync(T data, string id, string partitionKey)
        {
            try
            {
                await container.ReplaceItemAsync(data, id, new PartitionKey(partitionKey));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                await CreateAsync(data, partitionKey);
            }
        }

        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                var itemLst = new List<T>();

                var queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.id = '{id}'");
                var queryResultSetIterator = container.GetItemQueryIterator<T>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var dataItem in currentResultSet) itemLst.Add(dataItem);
                }

                return itemLst.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;  //let the caller handle this as appropriate
            }
        }

        public async Task<List<T>> GetByFieldValueAsync(string fieldName, string lastName)
        {
            try
            {
                var itemLst = new List<T>();

                var queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.{fieldName} = '{lastName}'");
                var queryResultSetIterator = container.GetItemQueryIterator<T>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var dataItem in currentResultSet)
                        itemLst.Add(dataItem);
                }

                return itemLst;

            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;  //let the caller handle this as appropriate
            }
        }


        protected async Task DeleteAsync( string id, string partitionKey)
        {
            try
            {
                var response = await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;  //let the caller handle this as appropriate
            }
        }

        private async Task CreateAsync(T data, string partitionKey)
        {
            try
            {
                await container.CreateItemAsync<T>(data, new PartitionKey(partitionKey));
            }
            catch (Exception e)
            {
                Logger?.WriteError(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;  //let the caller handle this as appropriate
            }
        }

    }
}
