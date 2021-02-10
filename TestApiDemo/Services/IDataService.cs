using System.Collections.Generic;
using TestApiDemo.Enumerations;
using TestApiDemo.Models;

namespace TestApiDemo.Services
{
    public interface IDataService
    {
        IEnumerable<Inventory> Get();
        Inventory GetByName(string name);
        IEnumerable<Inventory> GetByCreated(CreatedFilter filter);
        IEnumerable<Inventory> GetByQuantityLevel(QuantityFilter filter);

        DemoResponse Delete(string name);
        DemoResponse Put(string name, Inventory inventory);
        DemoResponse Post(IEnumerable<Inventory> inventory);

    }
}
