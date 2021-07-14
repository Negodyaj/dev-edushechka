CREATE PROCEDURE dbo.Comment_SelectAllByUserId
	@UserId int
AS
BEGIN
	SELECT 
		c.Id,
		c.Text,
		c.Date,
		c.IsDeleted,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo,
		ur.RoleId as id
	FROM dbo.Comment c
		inner join [User] u on u.Id=c.UserId
		inner join User_Role ur on ur.UserId=u.Id
	WHERE (c.UserId = @UserId AND c.IsDeleted=0)
END