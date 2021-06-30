CREATE PROCEDURE [dbo].[Payment_Delete]
    @Id     int
AS
BEGIN
    UPDATE dbo.Payment
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id
END