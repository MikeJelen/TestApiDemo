declare @i int = (select ProductId from dbo.Product where Name = '<@Name>')
delete from dbo.ProductInventory where ProductId = @i
delete from dbo.Product where ProductId = @i