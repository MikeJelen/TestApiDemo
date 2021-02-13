using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using TestApiDemo.Enumerations;
using TestApiDemo.Exceptions;
using TestApiDemo.Helpers;
using TestApiDemo.Models;

namespace TestApiDemo.Services
{
    public class InventoryDataService : IDataService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static IMessagingHelper _messagingHelper;

        public InventoryDataService(IServiceCollection serviceCollection)
        {
            var services = serviceCollection.BuildServiceProvider();
            _messagingHelper = services.GetRequiredService<IMessagingHelper>();
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
                        (pi, d) => new Inventory
                        {
                            Name = pi.p.Name,
                            Quantity = (d == null) ? 0 : d.Quantity,
                            CreatedOn = pi.p.CreatedOn
                        });

                Log.Info($"Get (all) product => {result.ToList().Count} result(s).");
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
                        (pi, d) => new Inventory
                        {
                            Name = pi.p.Name,
                            Quantity = (d == null) ? 0 : d.Quantity,
                            CreatedOn = pi.p.CreatedOn
                        })
                    .FirstOrDefault(i => i.Name.Equals(name));

                Log.Info($"Get product => {name}.");
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
                var result = (filter switch
                {
                    QuantityFilter.Highest => GetInventoryByMaxQuantity(context),
                    QuantityFilter.Lowest => GetInventoryByMinQuantity(context),
                    _ => throw new NotImplementedException()
                }).ToList();

                Log.Info($"Get quantity => {filter} returned {result.ToList().Count} result(s).");
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
                var result = (filter switch
                {
                    CreatedFilter.Newest => GetInventoryByCreatedOn(context, context.Products.Max(p => p.CreatedOn)),
                    CreatedFilter.Oldest => GetInventoryByCreatedOn(context, context.Products.Min(p => p.CreatedOn)),
                    _ => throw new NotImplementedException()
                }).ToList();

                Log.Info($"Get created => {filter} returned {result.ToList().Count} result(s).");
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
                var start = DateTime.UtcNow;
                var response = new DemoResponse
                {
                    IsSuccessful = true
                };

                var context = new InventoryContext();
                context.Database.ExecuteSqlRaw($"Exec {Properties.Resources.StoredProcedure_DeleteProduct} @name = '{name}';");

                response.Message = $"Delete for product {name} successfully completed.";
                WriteProgressLogMessage(start, response.Message);
                WriteQueuedMessage("Delete", name);

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
            try
            {
                ValidatePayload(inventory, name);
                var start = DateTime.UtcNow;
                var response = new DemoResponse
                {
                    IsSuccessful = true
                };

                var context = new InventoryContext();
                context.Database.ExecuteSqlRaw($"Exec {Properties.Resources.StoredProcedure_UpsertProduct} @name = '{name}', @quantity = {inventory.Quantity}, @createdOn = '{inventory.CreatedOn}';");

                response.Message = $"Put for product {name} successfully completed.";
                WriteProgressLogMessage(start, response.Message);
                WriteQueuedMessage("Put", new List<Inventory> { inventory });

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
                var inventories = inventory as Inventory[] ?? inventory.ToArray();
                var start = DateTime.UtcNow;
                var messageBuilder = new StringBuilder();
                var context = new InventoryContext();
                var response = new DemoResponse
                {
                    IsSuccessful = true
                };
                
                foreach (var item in inventories)
                {
                    ValidatePayload(item);
                    context.Database.ExecuteSqlRaw(
                        $"Exec {Properties.Resources.StoredProcedure_UpsertProduct} @name = '{item.Name}', @quantity = {item.Quantity}, @createdOn = '{item.CreatedOn}';");
                    messageBuilder.Append("Post for product ").Append(item.Name).AppendLine(" successfully completed.");
                }

                response.Message = messageBuilder.ToString();
                WriteProgressLogMessage(start, response.Message);
                WriteQueuedMessage("Post", inventories );

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
                    (p, i) => new { p, i })
                .SelectMany(pi => pi.i.DefaultIfEmpty(),
                    (pi, d) => new Inventory
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
                        (pi, d) => new Inventory
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
                .Select(p => new Inventory
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
                        (product, inventory) => new Inventory
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

            var endTime = DateTime.UtcNow;
            var duration = endTime.Subtract(start);
            var builder = new StringBuilder();
            builder.Append(message).Append(" Execution started at ")
                .Append(start.ToString(dateFormat)).Append(" and ended at ")
                .Append(endTime.ToString(dateFormat)).Append(" for a total run time of ")
                .Append(duration.TotalSeconds).Append(" seconds.");
            Log.Info(builder.ToString());
        }

        private static void WriteQueuedMessage(string action, string name, int quantity = 0, DateTime? createdOn = null)
        {
            WriteQueuedMessage(action, new List<Inventory> 
            {
                new Inventory()
                {
                    Name = name,
                    Quantity = quantity,
                    CreatedOn = createdOn ?? DateTime.UtcNow
                }
            });
        }

        private static void WriteQueuedMessage(string action, IEnumerable<Inventory> inventories)
        {
            _messagingHelper.Produce(
                Properties.Resources.MessageServerUri, 
                Properties.Resources.MessageTopic,
                JsonSerializer.Serialize(new Message { Action = action, Inventories = inventories })
            );
        }

        private static void ValidatePayload(Inventory inventory, string name = null)
        {
            if ((!string.IsNullOrEmpty(name)) && (!name.Equals(inventory.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BadRequestException(
                    $"Name {name} does not match the name in the input json ({inventory.Name})");
            }

            if (inventory.Quantity < 0)
            {
                throw new BadRequestException(
                    $"Product {inventory.Name} must have a quantity greater than or equal to 0");
            }

            if (inventory.CreatedOn > DateTime.UtcNow)
            {
                throw new BadRequestException(
                    $"Product {inventory.Name} cannot have created on a date in the future (date provided: {inventory.CreatedOn})");
            }
        }

        #endregion
    }
}
