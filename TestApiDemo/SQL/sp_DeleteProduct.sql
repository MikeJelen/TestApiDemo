use MikeDemo
go

drop procedure if exists [dbo].[sp_DeleteProduct];
go

create procedure [dbo].[sp_DeleteProduct]
	@name varchar(200)
as
	
declare   @ErrorMessage varchar(255) = null
		, @ProductId int = null

begin try
	
	select @ProductId = ProductId from [dbo].[Product] p where p.Name = @name

	if (@ProductId is null)
		begin
			select	@ErrorMessage = 'Product ' + @name + ' not found'
			raiserror (@ErrorMessage, 16, 1 );
		end
	
	--begin transaction
	
	delete from [dbo].[ProductInventory] where ProductId = @ProductId
	delete from [dbo].[Product] where ProductId = @ProductId

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
