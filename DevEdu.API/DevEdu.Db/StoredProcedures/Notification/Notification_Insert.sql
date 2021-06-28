CREATE PROCEDURE [dbo].[Notification_Insert]
	@Text nvarchar(max)
AS
	INSERT INTO [Notification] ([Text])
	VALUES (@Text)
	SELECT @@IDENTITY