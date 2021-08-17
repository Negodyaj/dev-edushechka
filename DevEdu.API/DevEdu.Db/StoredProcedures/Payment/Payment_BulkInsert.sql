CREATE PROCEDURE [dbo].[Payment_BulkInsert]
	@tblPayment PaymentType readonly
as
Begin
	set nocount on;
	
	insert into [dbo].[Payment](Date,Sum,UserId,IsPaid)
	output inserted.Id
	select Date,Sum,UserId,IsPaid from @tblPayment
End