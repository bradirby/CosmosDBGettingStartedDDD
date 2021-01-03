using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmosDbRepository;

namespace MyCosmosDb.Tests
{
    public class ConsoleLogger : ICosmosDbRepositoryLogger
    {
        public void WriteDebug(string msg)
        {
            Console.WriteLine(msg);
        }

        public void WriteWarning(string msg)
        {
            Console.WriteLine(msg);
        }

        public void WriteError(Exception e, string msg)
        {
            Console.WriteLine($"{e.Message} - {msg}");
        }
    }
}
