CREATE PROCEDURE [dbo].[Payment_Update]
    @Id     int,
	@IsPaid bit,
    @Sum decimal(6,2)
AS
BEGIN
    UPDATE dbo.Payment
        SET
        [Sum] = @Sum,
        Date = getdate(),
        IsPaid = @IsPaid
    WHERE [Id] = @Id
END