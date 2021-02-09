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
	- Service not using "using clauses for the contexct cause of DI (ConfigureServices)


## DB Tables
1.  CREATE TABLE [dbo].[Product] (
		[ProductId] INT           IDENTITY (1, 1) NOT NULL,
		[Name]      VARCHAR (200) NOT NULL,
		[CreatedOn] DATETIME2 (7) NOT NULL
	);
	CREATE UNIQUE NONCLUSTERED INDEX [UIX_Product_Name]
		ON [dbo].[Product]([Name] ASC);



2.	CREATE TABLE [dbo].[ProductInventory] (
		[ProductInventoryId] INT           IDENTITY (1, 1) NOT NULL,
		[ProductId]          INT           NOT NULL,
		[Quantity]           INT           NOT NULL,
		[CreatedOn]          DATETIME2 (7) NOT NULL,
		[LastUpdateOn]       DATETIME2 (7) NOT NULL
	);


