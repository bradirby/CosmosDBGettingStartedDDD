using System.Linq;
using CosmosDbRepository;
using Microsoft.Azure.Cosmos;

namespace MySampleCosmosDb
{
    public class MyCosmosDB : BaseCosmosDB
    {
        private Container MainContainer;
        private ICosmosDbRepositoryLogger Logger;
        
        public MyCosmosDB(string endPointUri, string primaryKey, ICosmosDbRepositoryLogger log = null) : base(log)
        {
            Logger = log;
            var desc = new MyCosmosDbDescriptor();
            InitAsync(endPointUri, primaryKey, desc).GetAwaiter().GetResult();
            MainContainer = Containers.FirstOrDefault();
        }

        public MyFamilyRepository GetFamilyRepository()
        {
            var repo = new MyFamilyRepository(MainContainer, Logger);
            return repo;
        }
    }
}
