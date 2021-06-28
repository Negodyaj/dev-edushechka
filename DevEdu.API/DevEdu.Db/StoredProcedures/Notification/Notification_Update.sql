CREATE PROCEDURE [dbo].[Notification_Update]
    @Id int,
	@Text nvarchar(max)
AS
    UPDATE [Notification]
    SET
    [Text] = @Text
    WHERE [Id] = @Id