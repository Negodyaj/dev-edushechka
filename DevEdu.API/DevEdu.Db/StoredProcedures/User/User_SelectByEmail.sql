CREATE PROCEDURE dbo.User_SelectByEmail
	@Email nvarchar(50)
AS
BEGIN
	SELECT 
		U.Id,
		U.FirstName,
		U.LastName,
		U.Patronymic,
		U.Email,
		U.Password,
		U.Username,
		U.IsDeleted,
		U.RegistrationDate,
		U.ContractNumber,
		U.BirthDate,
		U.GitHubAccount,
		U.Photo,
		U.PhoneNumber,
		u.ExileDate,
		U.CityId	as id,
		UR.RoleId	as id
	FROM dbo.[User] U WITH (NOLOCK)
	INNER JOIN dbo.User_Role ur WITH (NOLOCK) ON UR.UserId = U.Id
	WHERE U.Email = @Email
END