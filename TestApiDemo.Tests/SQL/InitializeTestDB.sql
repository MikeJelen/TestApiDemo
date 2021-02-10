use MikeDemo
go

/*
-------------------------------------------------------------------------------------
-- Table definitions
-------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Product] (
    [ProductId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (200) NOT NULL,
    [CreatedOn] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Product_Name]
    ON [dbo].[Product]([Name] ASC);
GO

CREATE TABLE [dbo].[ProductInventory] (
    [ProductInventoryId] INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]          INT           NOT NULL,
    [Quantity]           INT           DEFAULT ((0)) NOT NULL,
    [CreatedOn]          DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [LastUpdateOn]       DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductInventoryId] ASC),
    CONSTRAINT [FK_ProductInventory_ToProduct] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);
GO
*/

truncate table [dbo].[ProductInventory]
delete from [dbo].[Product] 
dbcc checkident ('Product', reseed, 1)  --Do this rather than dropping and recreating the constraint

-------------------------------------------------------------------------------------
-- Populate the tables
-------------------------------------------------------------------------------------
set nocount on
declare @productTable table (
    Id int identity(1,1) primary key,
    ProductName varchar(200)
)

insert into @productTable (ProductName) values
('Apples'), ('Pears'), ('Oranges'), ('Cherries'), ('Apricots')

declare @name varchar(200), 
        @id int = 0,
        @idx int = 0, 
        @rows int = (select count(*) from  @productTable)

while (@idx < @rows)
begin
    select @idx = @idx + 1
    select @name = ProductName from @productTable where id = @idx
    insert into [dbo].[Product] ([Name]) values (@name)
    select @id = scope_identity()
    insert into [dbo].[ProductInventory] ([ProductId], [Quantity]) values (@id, abs(checksum(NewId())) % 501)
    waitfor delay '00:00:01'    --Want different times for the created on
end

-------------------------------------------------------------------------------------
-- Check the results of the data creation
-------------------------------------------------------------------------------------
select	p.[Name], isnull(i.[Quantity], 0) as [Quantity], p.[CreatedOn]
from	dbo.Product p
		left outer join dbo.ProductInventory i on p.ProductId = i.ProductId

        