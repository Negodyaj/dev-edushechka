CREATE PROCEDURE dbo.User_SelectAll
AS
BEGIN
	SELECT 
		u.FirstName,
		u.LastName,
		u.Email,
		u.IsDeleted,
		u.RegistrationDate,
		u.ContractNumber,
		u.Photo,
		u.PhoneNumber,
		c.Id,
		c.Name
	FROM dbo.[User] u
	left join dbo.[City] c on u.CityId=c.Id
	WHERE IsDeleted = 0
END