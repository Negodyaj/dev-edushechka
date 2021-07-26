CREATE PROCEDURE [dbo].[Notification_Insert]
	@RoleId		int = null,
	@UserId		int = null,
	@GroupId	int = null,
	@Text		nvarchar(max)

AS
BEGIN
	INSERT INTO dbo.Notification (Text, Date, RoleId, UserId, GroupId)
	VALUES (@Text, getdate(), @RoleId, @UserId, @GroupId)
	SELECT @@IDENTITY
END