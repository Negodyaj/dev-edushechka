CREATE PROCEDURE [dbo].[Notification_Insert]
	@UserId		int = null,
	@RoleId		int = null,
	@Text		nvarchar(max)

AS
BEGIN
	INSERT INTO dbo.Notification (Text, Date, UserId, RoleId)
	VALUES (@Text, getdate(),@UserId,@RoleId)
	SELECT @@IDENTITY
END