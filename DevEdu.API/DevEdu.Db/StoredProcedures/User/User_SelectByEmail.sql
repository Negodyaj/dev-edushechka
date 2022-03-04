CREATE PROCEDURE dbo.User_SelectByEmail
	@Email nvarchar(50)
AS
BEGIN
	SELECT 
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo,
		u.IsDeleted,
		u.Password,
		u.CityId as Id,
		ur.RoleId as Id
	FROM dbo.[User] u
	inner join dbo.User_Role ur on ur.UserId = u.Id
	WHERE U.Email = @Email
END