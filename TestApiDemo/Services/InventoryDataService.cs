using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TestApiDemo.Enumerations;
using TestApiDemo.Exceptions;
using TestApiDemo.Models;

namespace TestApiDemo.Services
{
    public class InventoryDataService : IDataService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
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

        public DemoResponse Delete(string name)
        {
            try
            {
                var start = DateTime.Now;
                var response = new DemoResponse() { IsSuccessful = true };

                var context = new InventoryContext();
                context.Database.ExecuteSqlRaw($"Exec dbo.sp_DeleteProduct @name = '{name}';");

                response.Message = $"Delete for product {name} successfully completed";
                WriteProgressLogMessage(start, response.Message);

                return response;
            }
            catch (Exception e)
            {
                Log.Error($"Error on Delete: => {e.Message}");
                throw;
            }
        }

        public DemoResponse Put(string name, Inventory inventory)
        {
            if (!name.Equals(inventory.Name, StringComparison.OrdinalIgnoreCase))
            {
                var message =
                    $"Name ({name}) does not match the name in the input json ({inventory.Name})";

                Log.Error($"Error on Put: => {message}");
                throw new BadRequestException(message);
            }
            
            try
            {
                var start = DateTime.Now;
                var response = new DemoResponse() { IsSuccessful = true };

                var context = new InventoryContext();
                context.Database.ExecuteSqlRaw($"Exec dbo.sp_UpsertProduct @name = '{name}', @quantity = {inventory.Quantity}, @createdOn = '{inventory.CreatedOn}';");

                response.Message = $"Put for product {name} successfully completed )";
                WriteProgressLogMessage(start, response.Message);

                return response;
            }
            catch (Exception e)
            {
                Log.Error($"Error on Put: => {e.Message}");
                throw;
            }
        }
        
        public DemoResponse Post(IEnumerable<Inventory> inventory)
        {
            try
            {
                var start = DateTime.Now;
                var response = new DemoResponse() { IsSuccessful = true };
                var messageBuilder = new StringBuilder();

                var context = new InventoryContext();
                foreach (var item in inventory)
                {
                    context.Database.ExecuteSqlRaw(
                        $"Exec dbo.sp_UpsertProduct @name = '{item.Name}', @quantity = {item.Quantity}, @createdOn = '{item.CreatedOn}';");
                    messageBuilder.Append($"Post for product ").Append(item.Name).AppendLine(" successfully completed");
                }

                response.Message = messageBuilder.ToString();
                WriteProgressLogMessage(start, response.Message);

                return response;
            }
            catch (Exception e)
            {
                Log.Error($"Error on Post: => {e.Message}");
                throw;
            }
        }

        #region Helper Functions

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

        private static void WriteProgressLogMessage(DateTime start, string message)
        {
            const string dateFormat = "HH:mm:ss.fffffffK";

            var endTime = DateTime.Now;
            var duration = endTime.Subtract(start);
            var builder = new StringBuilder();
            builder.Append(message).Append(" Execution started at ")
                .Append(start.ToString(dateFormat)).Append(" and ended at ")
                .Append(endTime.ToString(dateFormat)).Append(" for a total run time of ")
                .Append(duration.TotalSeconds).Append(" seconds.");
            Log.Info(builder.ToString());
        }

        #endregion


    }
}
