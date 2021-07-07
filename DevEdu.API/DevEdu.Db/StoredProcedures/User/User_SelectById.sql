CREATE PROCEDURE dbo.[User_SelectById]
	@Id int
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
	WHERE [Id] = @Id
END