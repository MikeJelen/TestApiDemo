# TestApiDemo

## Business Functionality
1. Provide functionality for the user to query the inventory based on
	- Name 
	- Highest quantity
	- Lowest quantity
	- Oldest item
	- Newest item
	- Port utilization
	- Additional functionality

2. Allow the service to handle multiple requests
	- 2 posts
	- 2 deletes
	- Logic to handle edge cases

## Talking points
1. Seperate tables (in this case) is overkill but allows the opportunity to demo joins
2. EF is easy to work with in an example like this but not necessary
	- Delete/Insert/Update use sprocs through EF
3. Service not using "using" clauses for the contexct because of DI (ConfigureServices)
4. Incorporating:
	- NLog - Logging
	- NUnit - Testing
	- Confluent.Kafka - (Indirectly) Messaging - See project TestKafkaDemo for test utility and the Nuget package
	- Swagger (Swashbuckle) - as API execution tool in dev/test
5. Other cool API packages
	- https://devblogs.microsoft.com/aspnet/open-source-http-api-packages-and-tools/

## Notes
1. Scaffolding (https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)
	- Scaffold-DbContext "Server=(localdb)\ProjectsV13;Database=MikeDemo;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
	- The rename MikeDemoContext to InventoryContext
