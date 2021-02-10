declare @table table (
	[Name] varchar(200) primary key,
	[Quantity] int,
	[CreatedOn] varchar(50)
)

insert into @table
select	  p.[Name]					as 'Name'
		, isnull(i.[Quantity], 0)	as 'Quantity'
		, replace(rtrim(replace(convert(varchar(50), p.[CreatedOn], 126),'0',' ')),' ','0')
									as 'CreatedOn'	--removes trailing zeros in the milliseconds
from	dbo.Product p
		left outer join dbo.ProductInventory i on p.ProductId = i.ProductId

declare @min int = (select min(Quantity) from @table)
select	*
from	@table
where	Quantity = @min
for json path
