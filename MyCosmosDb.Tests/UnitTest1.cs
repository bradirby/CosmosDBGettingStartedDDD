using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MyCosmosDb.Tests
{
    public class Tests
    {
        private MyCosmosDB db;
        private IMyFamilyRepository FamRepo;
        private TestDataGenerator TstData = new TestDataGenerator();
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var uri =  "https://localhost:8081";
            var primaryKey = "put your value here";
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