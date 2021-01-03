using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository
{
    public interface ICosmosDbRepositoryLogger
    {
        void WriteDebug(string msg);
        void WriteWarning(string msg);
        void WriteError(Exception e, string msg);
    }
}
