using System.Collections.Generic;
using TestApiDemo.Enumerations;
using TestApiDemo.Models;

namespace TestApiDemo.Services
{
    public interface IDataService
    {
        void Delete(string name);

        IEnumerable<Inventory> Get();
        Inventory GetByName(string name);

        IEnumerable<Inventory> GetByCreated(CreatedFilter filter);
        IEnumerable<Inventory> GetByQuantityLevel(QuantityFilter filter);

        void Put(string name, Inventory inventory);

        void Post(IEnumerable<Inventory> inventory);

    }
}
