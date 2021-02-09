using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        // GET: api/Inventory
        [HttpGet]
        public IEnumerable<Inventory> Get()
        {
            return _dataService.Get();
        }

        // GET: api/Inventory/Apples
        [HttpGet("{name}", Name = "Name")]
        public Inventory Get(string name)
        {
            return _dataService.GetByName(name);
        }

        // POST: api/Inventory
        [HttpPost]
        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // PUT: api/Inventory/Apples
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            throw new NotImplementedException();
        }
    }
}
