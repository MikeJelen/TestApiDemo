insert into dbo.Product (Name) values ('<@Name>');
declare @i int = scope_identity()
insert into dbo.ProductInventory (ProductId, Quantity) values (@i, 50)