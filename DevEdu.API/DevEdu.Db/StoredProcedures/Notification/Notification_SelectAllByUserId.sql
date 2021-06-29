CREATE PROCEDURE [dbo].[Notification_SelectAllByUserId]
	@UserId		int
AS
BEGIN
	SELECT Id, Text, Date, UserId, RoleId, IsDeleted
	FROM dbo.Notification
	WHERE ([UserId] = @UserId AND [IsDeleted]=0)
END