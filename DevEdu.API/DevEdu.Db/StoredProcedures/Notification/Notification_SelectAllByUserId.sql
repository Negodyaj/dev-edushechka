CREATE PROCEDURE [dbo].[Notification_SelectAllByUserId]
	@UserId		int
AS
BEGIN
	SELECT
	n.Id, 
	n.Text,
	n.Date,
	n.IsDeleted,
	n.RoleId as Id,
	u.Id	
	FROM dbo.Notification n
		left join [User] u on u.Id=n.UserId
	WHERE (n.UserId = @UserId AND n.IsDeleted=0)
END