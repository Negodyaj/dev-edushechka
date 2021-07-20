CREATE PROCEDURE [dbo].[Notification_SelectById]
	@Id		int
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
	WHERE (n.Id = @Id)
END