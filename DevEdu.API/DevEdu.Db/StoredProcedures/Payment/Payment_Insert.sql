CREATE PROCEDURE [dbo].[Payment_Insert]
	@Date		datetime,
	@UserId		int,
	@Sum		decimal(6,2),
	@IsPaid		bit


AS
BEGIN
	INSERT INTO dbo.Payment (Date, UserId, Sum,  IsPaid)
	VALUES (@Date, @UserId, @Sum,  @IsPaid)
	SELECT @@IDENTITY
END