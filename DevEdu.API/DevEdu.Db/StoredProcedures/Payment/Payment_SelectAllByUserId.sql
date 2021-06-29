CREATE PROCEDURE [dbo].[Payment_SelectAllByUserId]
	@UserId		int
AS
BEGIN
	SELECT Id, Date, Sum, UserId, IsPaid, IsDeleted
	FROM dbo.Payment
	WHERE ([UserId] = @UserId AND [IsDeleted]=0)
END