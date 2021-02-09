
select	  top 1 p.[Name]				
from	dbo.Product p
		inner join dbo.ProductInventory i on p.[ProductId] = i.[ProductId]
order by p.[CreatedOn] desc
