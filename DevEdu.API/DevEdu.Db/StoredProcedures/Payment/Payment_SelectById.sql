CREATE PROCEDURE [dbo].[Payment_SelectById]
	@Id		int
AS
BEGIN
	SELECT Id, Date, Sum, UserId, IsPaid, IsDeleted
	FROM dbo.Payment
	WHERE ([Id] = @Id AND [IsDeleted]=0)
END