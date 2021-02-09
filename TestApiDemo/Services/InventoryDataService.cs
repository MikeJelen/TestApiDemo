using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using TestApiDemo.Enumerations;
using TestApiDemo.Models;

namespace TestApiDemo.Services
{
    public class InventoryDataService : IDataService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Delete(string name)
        {
            try
            {
                var context = new InventoryContext();
                var id = GetIdFromName(context, name);

                context.ProductInventories.RemoveRange(
                    context.ProductInventories.Where(p => p.ProductId.Equals(id))
                );

                context.Products.RemoveRange(
                    context.Products.Where(p => p.ProductId.Equals(id))
                );

                context.SaveChanges();
                Log.Info($"Deleted product => {name}");
            }
            catch (Exception e)
            {
                Log.Error($"Error on deletion: => {e.Message}");
                throw;
            }
            
        }

        public IEnumerable<Inventory> Get()
        {
            try
            {
                var context = new InventoryContext();
                var result = (context.Products
                    .GroupJoin(context.ProductInventories,
                        p => p.ProductId,
                        i => i.ProductId,
                        (p, i) => new { p, i }))
                    .SelectMany(pi => pi.i.DefaultIfEmpty(),
                        (pi, d) => new Inventory()
                        {
                            Name = pi.p.Name,
                            Quantity = (d == null) ? 0 : d.Quantity,
                            CreatedOn = pi.p.CreatedOn
                        });

                Log.Info($"Get (all) product => {result.Count()} result(s)");
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Error on Get (all): => {e.Message}");
                throw;
            }
            
        }

        public Inventory GetByName(string name)
        {
            try
            {
                var context = new InventoryContext();
                var result = (context.Products
                    .GroupJoin(context.ProductInventories,
                        p => p.ProductId,
                        i => i.ProductId,
                        (p, i) => new { p, i }))
                    .SelectMany(pi => pi.i.DefaultIfEmpty(),
                        (pi, d) => new Inventory()
                        {
                            Name = pi.p.Name,
                            Quantity = (d == null) ? 0 : d.Quantity,
                            CreatedOn = pi.p.CreatedOn
                        })
                    .FirstOrDefault(i => i.Name.Equals(name));

                Log.Info($"Get product => {name}");
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Error on get by name: => {e.Message}");
                throw;
            }
        }

        public IEnumerable<Inventory> GetByQuantityLevel(QuantityFilter filter)
        {
            try
            {
                var context = new InventoryContext();
                var result = filter switch
                {
                    QuantityFilter.Highest => GetInventoryByMaxQuantity(context),
                    QuantityFilter.Lowest => GetInventoryByMinQuantity(context),
                    _ => throw new NotImplementedException(),
                };

                Log.Info($"Get quantity => {filter}");
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Error on get quantity level: => {e.Message}");
                throw;
            }
        }

        public IEnumerable<Inventory> GetByCreated(CreatedFilter filter)
        {
            try
            {
                var context = new InventoryContext();
                var result = filter switch
                {
                    CreatedFilter.Newest => GetInventoryByCreatedOn(context, context.Products.Max(p => p.CreatedOn)),
                    CreatedFilter.Oldest => GetInventoryByCreatedOn(context, context.Products.Min(p => p.CreatedOn)),
                    _ => throw new NotImplementedException(),
                };

                Log.Info($"Get created => {filter}");
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Error on get by created: => {e.Message}");
                throw;
            }
        }

        public void Put(string name, Inventory inventory)
        {
            if (!name.Equals(inventory.Name))
            {
                var message =
                    $"Invalid payload. Name ({name}) does not match the name in the input json ({inventory.Name})";
                Log.Error($"Error on put: => {message}");
                throw new Exception(message);
            }
            
            try
            {
                var context = new InventoryContext();
                var product = (context.Products.Where(p => p.Name.Equals(name))).FirstOrDefault();

                if (product == null)
                {
                    InsertProduct(context, name, inventory.Quantity);
                }
                else
                {
                    UpsertProductInventory(context, product.ProductId, inventory.Quantity);
                }

                context.SaveChanges();
                Log.Info($"Put completed => {name}");
            }
            catch (Exception e)
            {
                Log.Error($"Error on put: => {e.Message}");
                throw;
            }

        }

        public void Post(IEnumerable<Inventory> inventory)
        {
            try
            {
                var processedItems = new List<string>();
                var context = new InventoryContext();
                foreach (var item in inventory)
                {
                    processedItems.Add(item.Name);
                    var product = (context.Products.Where(p => p.Name.Equals(item.Name))).FirstOrDefault();
                    if (product == null)
                    {
                        InsertProduct(context, item.Name, item.Quantity);
                    }
                    else
                    {
                        UpsertProductInventory(context, product.ProductId, item.Quantity);
                    }
                }


                context.SaveChanges();
                Log.Info($"Put post => {string.Join(",", processedItems)}");
            }
            catch (Exception e)
            {
                Log.Error($"Error on put: => {e.Message}");
                throw;
            }
        }

        
        #region Helper Functions for CRUD Operations 

        private static int GetIdFromName(InventoryContext context, string name)
        {
            return (context.Products
                .Where(p => p.Name.Equals(name))
                .Select(p => p.ProductId))
                .SingleOrDefault();
        }
        
        private static IEnumerable<Inventory> GetInventoryByCreatedOn(InventoryContext context, DateTime createdOn)
        {
            return context.Products
                .GroupJoin(context.ProductInventories,
                    p => p.ProductId,
                    i => i.ProductId,
                    (p, i) => new {p, i})
                .SelectMany(pi => pi.i.DefaultIfEmpty(),
                    (pi, d) => new Inventory()
                    {
                        Name = pi.p.Name,
                        Quantity = (d == null) ? 0 : d.Quantity,
                        CreatedOn = pi.p.CreatedOn
                    })
                .Where(inventory => inventory.CreatedOn.Equals(createdOn));
        }

        private static IEnumerable<Inventory> GetInventoryByMaxQuantity(InventoryContext context)
        {
            var quantity = context.ProductInventories.Max(p => p.Quantity);

            return (context.Products
                    .GroupJoin(context.ProductInventories,
                        p => p.ProductId,
                        i => i.ProductId,
                        (p, i) => new { p, i })
                    .SelectMany(pi => pi.i.DefaultIfEmpty(),
                        (pi, d) => new Inventory()
                        {
                            Name = pi.p.Name,
                            Quantity = (d == null) ? 0 : d.Quantity,
                            CreatedOn = pi.p.CreatedOn
                        })
                    .Where(i => i.Quantity.Equals(quantity)));
        }

        private static IEnumerable<Inventory> GetInventoryByMinQuantity(InventoryContext context)
        {
            // Get a list of items that have no records in the inventory table - this means 0 quantity because its a new product
            var results = (context.Products
                .Where(p => !context.ProductInventories.Any(i => p.ProductId.Equals(i.ProductId)))
                .Select(p => new Inventory()
                {
                    Name = p.Name,
                    Quantity = 0,
                    CreatedOn = p.CreatedOn
                }))
                .ToList();

            // Get The minimum in the quantity table (when no records from previous get min from table)
            var min = (results.Count == 0) 
                ? context.ProductInventories.Min(i => i.Quantity)
                : 0;

            // Add items in the quantity table that have the min quantity 
            results.AddRange(context.Products
                    .Join(context.ProductInventories,
                        product => product.ProductId,
                        inventory => inventory.ProductId,
                        (product, inventory) => new Inventory()
                        {
                            Name = product.Name,
                            Quantity = inventory.Quantity,
                            CreatedOn = product.CreatedOn
                        })
                    .Where(i => i.Quantity.Equals(min))
                .ToList());

            return results;
        }

        private static void InsertProduct(InventoryContext context, string name, int quantity)
        {
            var product = new Product()
            {
                Name = name,
                ProductInventories = new List<ProductInventory>() 
                    { new ProductInventory() { Quantity = quantity } }
            };
            
            context.Products.Add(product);
            context.SaveChanges();
        }

        private static void UpsertProductInventory(InventoryContext context, int id, int quantity)
        {
            var productInventory = (context.ProductInventories
                .Where(p => p.ProductId.Equals(id))).SingleOrDefault();

            if (productInventory == null)
            {
                productInventory = new ProductInventory() {ProductId = id, Quantity = quantity};
                context.ProductInventories.Add(productInventory);
            }
            else
            {
                productInventory.Quantity = quantity;
            }

            
            context.SaveChanges();
        }

        #endregion


    }
}
