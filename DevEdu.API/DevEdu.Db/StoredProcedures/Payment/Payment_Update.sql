CREATE PROCEDURE [dbo].[Payment_Update]
    @Id     int,
	@IsPaid bit,
    @Sum decimal(6,2),
    @Date date
AS
BEGIN
    UPDATE dbo.Payment
        SET
        IsPaid = @IsPaid,
        [Sum] = @Sum,
        Date = @Date
    WHERE [Id] = @Id
END