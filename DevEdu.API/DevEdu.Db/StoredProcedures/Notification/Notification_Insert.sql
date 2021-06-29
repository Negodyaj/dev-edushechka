CREATE PROCEDURE [dbo].[Notification_Insert]
	@UserId		int,
	@RoleId		int,
	@Text		nvarchar(max)

AS
BEGIN
	INSERT INTO dbo.Notification (Text, Date, UserId, RoleId)
	VALUES (@Text, getdate(),@UserId,@RoleId)
	SELECT @@IDENTITY
END