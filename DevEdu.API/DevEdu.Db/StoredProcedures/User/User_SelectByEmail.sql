CREATE PROCEDURE dbo.User_SelectByEmail
	@Email nvarchar(50)
AS
BEGIN
	SELECT 
		u.Id,
		u.FirstName,
		u.LastName,
		u.Patronymic,
		u.Email,
		u.Password,
		u.Username,
		u.IsDeleted,
		u.RegistrationDate,
		u.ContractNumber,
		u.BirthDate,
		u.GitHubAccount,
		u.Photo,
		u.PhoneNumber,
		u.ExileDate,
		u.CityId as id,
		ur.RoleId as id
	FROM dbo.[User] u 
	inner join dbo.User_Role ur on ur.UserId = u.Id
	WHERE u.Email = @Email
END