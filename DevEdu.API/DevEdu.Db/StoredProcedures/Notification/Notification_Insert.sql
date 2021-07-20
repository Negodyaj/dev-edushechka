CREATE PROCEDURE [dbo].[Notification_Insert]
	@RoleId		int,
	@UserId		int,
	@GroupId	int,
	@Text		nvarchar(max)

AS
BEGIN
	INSERT INTO dbo.Notification (Text, Date, RoleId, UserId, GroupId)
	VALUES (@Text, getdate(), @UserId, @RoleId, @GroupId)
	SELECT @@IDENTITY
END