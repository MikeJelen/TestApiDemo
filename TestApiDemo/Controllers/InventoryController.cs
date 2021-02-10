using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApiDemo.Enumerations;
using TestApiDemo.Models;
using TestApiDemo.Services;

namespace TestApiDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IDataService _dataService;
        public InventoryController(IDataService dataService)
        {
            _dataService = dataService;
        }

        #region Get Functions

        // GET: inventory
        [HttpGet]
        public Task<IEnumerable<Inventory>> Get()
        {
            return Task.FromResult(_dataService.Get());
        }

        // GET: inventory/Apples
        [HttpGet("{name}", Name = "Name")]
        public Task<Inventory> Get(string name)
        {
            return Task.FromResult(_dataService.GetByName(name));
        }

        // GET: inventory/created/Oldest
        [HttpGet]
        [Route("created/{filter}", Name = "GetByCreated")]
        public Task<IEnumerable<Inventory>> GetByCreated(CreatedFilter filter)
        {
            return Task.FromResult(_dataService.GetByCreated(filter));
        }

        // GET: inventory/quantity/Highest
        [HttpGet]
        [Route("quantity/{filter}", Name = "GetByQuantityLevel")]
        public Task<IEnumerable<Inventory>> GetByQuantityLevel(QuantityFilter filter)
        {
            return Task.FromResult(_dataService.GetByQuantityLevel(filter));
        }

        #endregion

        // POST: inventory
        [HttpPost]
        public Task<DemoResponse> Post([FromBody] IEnumerable<Inventory> value)
        {
            return Task.FromResult(_dataService.Post(value));

        }

        // PUT: inventory/Apples
        [HttpPut("{name}")]
        public Task<DemoResponse> Put(string name, [FromBody] Inventory value)
        {
            return Task.FromResult(_dataService.Put(name, value));
        }

        // DELETE: inventory/Apples
        [HttpDelete("{name}")]
        public Task<DemoResponse> Delete(string name)
        {
            return Task.FromResult(_dataService.Delete(name));
        }
    }
}
