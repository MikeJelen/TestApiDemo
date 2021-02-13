use MikeDemo
go

drop procedure if exists [dbo].[sp_UpsertProduct];
go

create procedure [dbo].[sp_UpsertProduct]
	  @name varchar(200)
	, @quantity int
	, @createdOn DateTime2 = null
as
	
declare   @ErrorMessage varchar(255) = null
		, @ProductId int = null

begin try
	
	if (@createdOn is null)
		begin
			set @createdOn = getutcdate()
		end

	
	
	--begin transaction

		merge [dbo].[Product] as target
				using (select @name) as source ([Name])
				on (target.[Name] = source.[Name])
				when not matched then
					insert ([Name], [CreatedOn])
						values (@name, @createdOn);

		select @ProductId = ProductId from [dbo].[Product] p where p.Name = @name

		merge [dbo].[ProductInventory] as target
				using (select @ProductId) as source ([ProductId])
				on (target.[ProductId] = source.[ProductId])
				when matched then 
					update set [Quantity] = @quantity, [LastUpdateOn] = @createdOn
				when not matched then
					insert ([ProductId], [Quantity], [CreatedOn], [LastUpdateOn])
						values (@ProductId, @quantity, @createdOn, @createdOn);

	--commit transaction 

end try

begin catch

	--if (@@trancount > 0)
	--	begin
	--		rollback transaction
	--	end

	select @ErrorMessage = ERROR_MESSAGE()
	raiserror (@ErrorMessage, 16, 1 )

end catch
