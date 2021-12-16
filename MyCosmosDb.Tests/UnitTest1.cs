using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CosmosDbRepository;
using NUnit.Framework;

namespace MySampleCosmosDb.Tests
{
    public class Tests
    {

        //to start, download the cosmosdb emulator from here: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21
        private MyCosmosDB db;
        private IMyFamilyRepository FamRepo;
        private readonly TestDataGenerator TstData = new TestDataGenerator();
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //these come from the webpage that is displayed when you run the Cosmos local emulator
            var uri =  "https://localhost:8081";
            var primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            db = new MyCosmosDB(uri, primaryKey);
            FamRepo = db.GetFamilyRepository();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await db.DeleteDatabaseAsync();
        }

        [Test]
        public void DbCreatedSuccessfully()
        {
            Assert.IsNotNull(db);
        }

        [Test]
        public void RepositoryCreatedSuccessfully()
        {
            Assert.IsNotNull(FamRepo);
        }

        [Test]
        public async Task SaveRandomFamilySucceeds()
        {
            var fam = TstData.CreateRandomFamily("SaveRandomFamilySucceeds");
            await FamRepo.SaveAsync(fam);
        }

        [Test]
        public async Task GetByLastName_RetrievesSavedFamily()
        {
            var lastname = "GetByLastName_RetrievesSavedFamily";
            
            var fam = TstData.CreateRandomFamily(lastname);
            fam.Address.City = Guid.NewGuid().ToString();
            await FamRepo.SaveAsync(fam);
            
            var savedFamLst = await FamRepo.GetByLastName(lastname);
            Assert.AreEqual(1, savedFamLst.Count);
            
            var savedFam = savedFamLst.First();
            Assert.IsNotNull(savedFam);
            Assert.AreEqual(fam.Address.City, savedFam.Address.City);
        }

        [Test]
        public async Task GetById_RetrievesSavedFamily()
        {
            var lastname = "GetById_RetrievesSavedFamily";
            var fam = TstData.CreateRandomFamily(lastname);
            await FamRepo.SaveAsync(fam);

            var savedFam = await FamRepo.GetByIdAsync(fam.Id);
            Assert.IsNotNull(savedFam);
        }

        [Test]
        public async Task DeleteAsync_DeletesFamily()
        {
            var lastname = "DeleteAsync_DeletesFamily";
            var fam = TstData.CreateRandomFamily(lastname);
            await FamRepo.SaveAsync(fam);

            var savedFam = await FamRepo.GetByIdAsync(fam.Id);
            Assert.IsNotNull(savedFam);

            await FamRepo.DeleteAsync(fam);

            savedFam = await FamRepo.GetByIdAsync(fam.Id);
            Assert.IsNull(savedFam);
        }

    }
}