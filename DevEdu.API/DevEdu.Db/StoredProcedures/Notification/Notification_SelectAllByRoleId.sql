CREATE PROCEDURE [dbo].[Notification_SelectAllByRoleId]
	@RoleId		int
AS
BEGIN
	SELECT
	n.Id, 
	n.Text,
	n.Date,
	n.IsDeleted,
	n.RoleId as Id
	FROM dbo.Notification n
	WHERE (n.RoleId = @RoleId AND n.IsDeleted=0)
END