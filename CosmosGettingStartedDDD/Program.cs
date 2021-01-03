using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using BoundedContext;
using MyCosmosDb;

namespace CosmosGettingStartedDDD
{
    class Program
    {

        public static async Task Main(string[] args)
        {
                Program p = new Program();
                await p.GetStartedDemoAsync();
        }

        /// <summary>
        /// Entry point to call methods that operate on Azure Cosmos DB resources in this sample
        /// </summary>
        private async Task GetStartedDemoAsync()
        {
            var log = new ConsoleLogger();
            var tstData = new TestDataGenerator();
            var anderson1 = tstData.CreateAndersonFamily();
            var wakfield1 = tstData.CreateWakefieldFamily();

            //create the db
            Console.WriteLine($"{DateTime.Now} Starting");
            var db = new MyCosmosDB(ConfigurationManager.AppSettings["EndPointUri"], ConfigurationManager.AppSettings["PrimaryKey"], log);
            Console.WriteLine($"{DateTime.Now} DB Created");
            var famRepo = db.GetFamilyRepository();
            Console.WriteLine($"{DateTime.Now} Repo Created");

            //save some families
            await famRepo.SaveAsync(anderson1);
            await famRepo.SaveAsync(wakfield1);
            Console.WriteLine($"{DateTime.Now} Families Saved");

            //query them back by id
            var anderson5 = await famRepo.GetByIdAsync(anderson1.Id);
            Console.WriteLine($"{DateTime.Now} Family Retrieved by ID");

            //query them back by lastname
            var andersonLst = await famRepo.GetByFieldValueAsync(nameof(anderson1.LastName), anderson1.LastName);
            var wakefieldLst = await famRepo.GetByFieldValueAsync(nameof(wakfield1.LastName), wakfield1.LastName);
            Console.WriteLine($"{DateTime.Now} Family Retrieved by name");

            var firstAnderson = andersonLst.First();
            firstAnderson.IsRegistered = true;
            await famRepo.SaveAsync(firstAnderson);
            Console.WriteLine($"{DateTime.Now} Family updated");

            await db.DeleteDatabaseAsync();
            Console.WriteLine($"{DateTime.Now} DB deleted");

        }


    }
}
