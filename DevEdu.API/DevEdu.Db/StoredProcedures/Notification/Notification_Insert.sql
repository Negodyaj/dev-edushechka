CREATE PROCEDURE [dbo].[Notification_Insert]
	@Text nvarchar(max)
AS
	INSERT INTO dbo.Notification ([Text], Date) -- add other columns
	VALUES (@Text, getdate())
	SELECT @@IDENTITY