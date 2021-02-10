# TestApiDemo

## Business Functionality
1. Provide functionality for the user to query the inventory based on
	- Name 
		- InventoryService.Get()
	- Highest quantity
	- Lowest quantity
	- Oldest item
	- Newest item
	- Port utilization
		- Properties\launchSettings.json
	- Additional functionality

2. Allow the service to handle multiple requests
	- 2 posts
	- 2 deletes
		- Logic to handle edge cases

## Testing
	- Test for each business case and NFR

## Talking points
1. Design Decisions
	- Seperate tables (in this case) is overkill but allows the opportunity for joins
	- EF is easy to work with in an example like this but not necessary
	- Service not using "using" clauses for the contexct cause of DI (ConfigureServices)


## DB Tables
1. Script for DB initialization
	- \TestApiDemo\TestApiDemo.Tests\SQL\InitializeTestDB.sql

## Notes
1. Scaffolding (https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)
	- Scaffold-DbContext "Server=(localdb)\ProjectsV13;Database=MikeDemo;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
	- The rename MikeDemoContext to InventoryContext
