using System.Linq;
using CosmosDbRepository;
using Microsoft.Azure.Cosmos;

namespace MyCosmosDb
{
    public class MyCosmosDB : BaseCosmosDB
    {
        private Container MainContainer;
        
        public MyCosmosDB(string endPointUri, string primaryKey)
        {
            var desc = new MyCosmosDbDescriptor();
            Init(endPointUri, primaryKey, desc).Wait();
            MainContainer = Containers.FirstOrDefault();
        }

        public MyFamilyRepository GetFamilyRepository()
        {
            var repo = new MyFamilyRepository(MainContainer);
            return repo;
        }
    }
}
