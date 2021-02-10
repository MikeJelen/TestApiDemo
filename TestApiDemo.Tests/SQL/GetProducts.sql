select	count(*)		
from	dbo.Product p
		inner join dbo.ProductInventory i on p.[ProductId] = i.[ProductId]
where	p.Name in (<@Name>)
