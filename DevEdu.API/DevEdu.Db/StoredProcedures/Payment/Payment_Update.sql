CREATE PROCEDURE [dbo].[Payment_Update]
	@Date		Datetime,
    @UserId int,
    @Id     int,
	@IsPaid bit,
    @Sum decimal(6,2)
AS
BEGIN
    UPDATE dbo.Payment
        SET
        Date = @Date,
        UserId=@UserId,
        IsPaid = @IsPaid,
        [Sum] = @Sum
    WHERE [Id] = @Id
END