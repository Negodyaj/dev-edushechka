CREATE PROCEDURE dbo.User_SelectByEmail
	@Email nvarchar(50)
AS
BEGIN
	SELECT 
		u.Id,
		u.IsDeleted,
		u.Password,
		ur.RoleId as Id
	FROM dbo.[User] u
	inner join dbo.User_Role ur on ur.UserId = u.Id
	WHERE U.Email = @Email
END