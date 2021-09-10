CREATE PROCEDURE dbo.User_SelectByEmail
	@Email nvarchar(50)
AS
BEGIN
	SELECT 
		U.Id,
		UR.RoleId as id
	FROM dbo.[User] U WITH (NOLOCK)
	INNER JOIN dbo.User_Role ur WITH (NOLOCK) ON UR.UserId = U.Id
	WHERE U.Email = @Email
END