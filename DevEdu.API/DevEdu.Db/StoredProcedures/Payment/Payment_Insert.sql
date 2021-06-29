CREATE PROCEDURE [dbo].[Payment_Insert]
	@UserId		int,
	@Sum		decimal(6,2),
	@IsPaid		bit


AS
BEGIN
	INSERT INTO dbo.Payment (Sum, Date, UserId, IsPaid)
	VALUES (@Sum, getdate(), @UserId, @IsPaid)
	SELECT @@IDENTITY
END