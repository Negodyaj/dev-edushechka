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
	u.FirstName,
	u.LastName,
	u.GitHubAccount,
	u.Photo
	FROM dbo.Notification n
		inner join [User] u on u.Id=n.UserId
	WHERE (n.Id = @Id)
END