CREATE Procedure [dbo].[Payment_BulkInsert]
	@tblPayment PaymentType readonly
as
Begin
	set nocount on;
	
	insert into [dbo].[Payment](Date,Sum,UserId,IsPaid)
	output inserted.id
	select Date,Sum,UserId,IsPaid from @tblPayment
	
End