using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public IEnumerable<Inventory> Get()
        {
            return _dataService.Get();
        }

        // GET: inventory/Apples
        [HttpGet("{name}", Name = "Name")]
        public Inventory Get(string name)
        {
            return _dataService.GetByName(name);
        }

        // GET: inventory/created/Oldest
        [HttpGet]
        [Route("created/{filter}", Name = "GetByCreated")]
        public IEnumerable<Inventory> GetByCreated(CreatedFilter filter)
        {
            return _dataService.GetByCreated(filter);
        }

        // GET: inventory/quantity/Highest
        [HttpGet]
        [Route("quantity/{filter}", Name = "GetByQuantityLevel")]
        public IEnumerable<Inventory> GetByQuantityLevel(QuantityFilter filter)
        {
            return _dataService.GetByQuantityLevel(filter);
        }
        
        #endregion

        // POST: inventory
        [HttpPost]
        public void Post([FromBody] IEnumerable<Inventory> value)
        {
            _dataService.Post(value);
        }

        // PUT: inventory/Apples
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] Inventory value)
        {
            _dataService.Put(name, value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            _dataService.Delete(name);
        }
    }
}
