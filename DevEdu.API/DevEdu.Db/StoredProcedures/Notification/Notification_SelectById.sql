CREATE PROCEDURE [dbo].[Notification_SelectById]
	@Id		int
AS
BEGIN
	SELECT
	n.Id, 
	n.Text,
	n.Date,
	n.IsDeleted,
	u.Id,
	u.FirstName,
	u.LastName,
	u.GitHubAccount,
	u.Photo,
	ur.RoleId as id
	FROM dbo.Notification n
		inner join [User] u on u.Id=n.UserId
		inner join User_Role ur on ur.UserId=u.Id
	WHERE (n.Id = @Id)
END