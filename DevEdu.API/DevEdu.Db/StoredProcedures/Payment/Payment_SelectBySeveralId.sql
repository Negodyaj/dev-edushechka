CREATE PROCEDURE [dbo].[Payment_SelectBySeveralId]
	
@tblIds dbo.IdType readonly
as
Begin
	select 
		p.Id, 
		p.Date, 
		p.Sum, 
		p.IsPaid, 
		p.UserId as Id, 
		u.FirstName, 
		u.LastName, 
		u.Email, 
		u.Photo
from dbo.Payment p
	inner join @tblIds ids
	on p.Id = ids.Id
	inner join [dbo].[User] u
	on UserId = u.Id
End 