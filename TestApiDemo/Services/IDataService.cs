using System.Collections.Generic;
using TestApiDemo.Models;

namespace TestApiDemo.Services
{
    public interface IDataService
    {
        List<Inventory> Get();
        Inventory GetByName(string name);
        void Put();
        void Post(string name);
        void Delete(string name);
    }
}
