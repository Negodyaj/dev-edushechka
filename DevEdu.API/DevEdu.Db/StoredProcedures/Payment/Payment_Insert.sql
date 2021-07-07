CREATE PROCEDURE [dbo].[Payment_Insert]
	@UserId		int,
	@Sum		decimal(6,2),
	@IsPaid		bit,
	@Date		datetime


AS
BEGIN
	INSERT INTO dbo.Payment (Sum, Date, UserId, IsPaid)
	VALUES (@Sum, @Date, @UserId, @IsPaid)
	SELECT @@IDENTITY
END