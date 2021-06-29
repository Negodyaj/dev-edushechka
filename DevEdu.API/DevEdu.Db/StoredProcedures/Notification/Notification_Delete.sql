CREATE PROCEDURE [dbo].[Notification_Delete]
    @Id     int
AS
BEGIN
    UPDATE dbo.Notification
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id
END