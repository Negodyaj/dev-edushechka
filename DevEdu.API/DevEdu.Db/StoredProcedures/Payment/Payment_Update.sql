CREATE PROCEDURE [dbo].[Payment_Update]
    @Id     int,
	@IsPaid bit,
    @Sum decimal(6,2),
	@Date		datetime
AS
BEGIN
    UPDATE dbo.Payment
        SET
        [Sum] = @Sum,
        Date = @Date,
        IsPaid = @IsPaid
    WHERE [Id] = @Id
END