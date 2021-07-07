CREATE PROCEDURE dbo.User_SelectAll
AS
BEGIN
	SELECT 
		FisrtName,
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
		PhoneNumer,
		ExileDate
	FROM dbo.[User]
	WHERE IsDeleted = 0
END