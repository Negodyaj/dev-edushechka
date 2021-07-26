Create Procedure [dbo].[Insert_Payments]
	@tblPayment PaymentType readonly
as
Begin
	set nocount on;
	
	insert into [dbo].[Payment](Date,Sum,UserId,IsPaid,IsDeleted)
	output inserted.id
	select Date,Sum,UserId,IsPaid,IsDeleted from @tblPayment
	
End