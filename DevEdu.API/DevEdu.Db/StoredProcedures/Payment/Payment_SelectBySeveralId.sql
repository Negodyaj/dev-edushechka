CREATE PROCEDURE [dbo].[Payment_SelectBySeveralId]
	
@tblIds dbo.IdType readonly
as
Begin
	select p.Id, Date, Sum, UserId, IsPaid
from dbo.Payment p
	inner join @tblIds ids
	on p.Id = ids.Id
End