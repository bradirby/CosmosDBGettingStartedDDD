using System.Linq;
using CosmosDbRepository;
using Microsoft.Azure.Cosmos;

namespace MyCosmosDb
{
    public class MyCosmosDB : BaseCosmosDB
    {
        private Container MainContainer;
        private ICosmosDbRepositoryLogger Logger;
        
        public MyCosmosDB(string endPointUri, string primaryKey, ICosmosDbRepositoryLogger log) : base(log)
        {
            Logger = log;
            var desc = new MyCosmosDbDescriptor();
            Init(endPointUri, primaryKey, desc).Wait();
            MainContainer = Containers.FirstOrDefault();
        }

        public MyFamilyRepository GetFamilyRepository()
        {
            var repo = new MyFamilyRepository(MainContainer, Logger);
            return repo;
        }
    }
}
