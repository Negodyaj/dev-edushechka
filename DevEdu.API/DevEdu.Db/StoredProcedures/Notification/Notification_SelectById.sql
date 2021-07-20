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
	u.Id,
	g.Id
	FROM dbo.Notification n
		left join [User] u on u.Id = n.UserId
		left join [Group] g on g.Id = n.GroupId  
	WHERE (n.Id = @Id)
END