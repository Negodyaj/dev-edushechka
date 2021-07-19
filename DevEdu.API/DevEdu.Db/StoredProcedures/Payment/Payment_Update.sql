CREATE PROCEDURE [dbo].[Payment_Update]
    @Id     int,
	@IsPaid bit,
    @Sum decimal(6,2)
AS
BEGIN
    UPDATE dbo.Payment
        SET
        IsPaid = @IsPaid,
        [Sum] = @Sum
    WHERE [Id] = @Id
END