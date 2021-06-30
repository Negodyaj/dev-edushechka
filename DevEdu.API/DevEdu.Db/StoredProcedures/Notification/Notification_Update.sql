CREATE PROCEDURE [dbo].[Notification_Update]
    @Id     int,
	@Text   nvarchar(max)
AS
BEGIN
    UPDATE dbo.Notification
    SET
    [Text] = @Text
    WHERE [Id] = @Id
END