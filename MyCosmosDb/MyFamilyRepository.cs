using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDbRepository;
using Microsoft.Azure.Cosmos;

namespace MySampleCosmosDb
{
    public interface IMyFamilyRepository
    {
        Task SaveAsync(Family data);
        Task DeleteAsync(Family data);
        Task<List<Family>> GetByLastName(string lastname);
        Task<Family> GetByIdAsync(string id);
    }

    public class MyFamilyRepository : BaseCosmosDBRepository<Family>, IMyFamilyRepository
    {
        
        public MyFamilyRepository(Container c, ICosmosDbRepositoryLogger log) : base(c, log)
        {
        }

        public async Task SaveAsync(Family data)
        {
            await base.SaveAsync(data, data.Id, data.PartitionKey);
        }

        public async Task DeleteAsync(Family data)
        {
            await base.DeleteAsync(data.Id, data.PartitionKey);
        }

      
        public async Task<List<Family>> GetByLastName(string lastname)
        {
            return await GetByFieldValueAsync(nameof(Family.LastName), lastname);
        }

    }
}
