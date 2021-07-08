CREATE PROCEDURE dbo.User_SelectAll
AS
BEGIN
	SELECT 
		FirstName,
		LastName,
		Patronymic,
		Email,
		Username,
		Password,
		IsDeleted,
		RegistrationDate,
		ContractNumber,
		CityId,
		BirthDate,
		GitHubAccount,
		Photo,
		PhoneNumber,
		ExileDate
	FROM dbo.[User]
	WHERE IsDeleted = 0
END