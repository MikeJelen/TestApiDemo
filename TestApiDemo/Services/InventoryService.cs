using System;
using System.Collections.Generic;
using System.Linq;
using TestApiDemo.Models;

namespace TestApiDemo.Services
{
    public class InventoryService : IDataService
    {
        public List<Inventory> Get()
        {
            using (var context = new InventoryContext())
            {
                return (context.Products
                    .Join(context.ProductInventories,
                        product => product.ProductId,
                        inventory => inventory.ProductId,
                        (product, inventory) => new Inventory()
                        {
                            Name = product.Name,
                            Quantity = inventory.Quantity,
                            CreatedOn = inventory.CreatedOn
                        }
                    ).ToList());
            }
        }

        public Inventory GetByName(string name)
        {
            using (var context = new InventoryContext())
            {
                return context.Products
                    .Join(context.ProductInventories,
                        product => product.ProductId,
                        inventory => inventory.ProductId,
                        (product, inventory) => new Inventory()
                        {
                            Name = product.Name,
                            Quantity = inventory.Quantity,
                            CreatedOn = inventory.CreatedOn
                        })
                    .FirstOrDefault(i => i.Name.Equals(name));
            }
        }

        public void Put()
        {
            throw new NotImplementedException();
        }

        public void Post(string name)
        {

        }

        public void Delete(string name)
        {
            using (var context = new InventoryContext())
            {
                var id = GetIdFromName(context, name);

                context.ProductInventories.RemoveRange(
                    context.ProductInventories.Where(p=> p.ProductId.Equals(id))
                );

                context.Products.RemoveRange(
                    context.Products.Where(p => p.ProductId.Equals(id))
                );

                context.SaveChanges();
            }
        }

        #region Helper Functions

        private static int GetIdFromName(InventoryContext context, string name)
        {
            return (context.Products
                .Where(p => p.Name.Equals(name))
                .Select(p => p.ProductId))
                .FirstOrDefault();
        }

        #endregion
    }
}
