CREATE PROCEDURE dbo.User_SelectAll
AS
BEGIN
	SELECT 
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo,
		u.CityId as Id,
		ur.RoleId as Id
	FROM dbo.[User] u
	inner join dbo.User_Role ur on ur.UserId = u.Id
	WHERE IsDeleted = 0
END