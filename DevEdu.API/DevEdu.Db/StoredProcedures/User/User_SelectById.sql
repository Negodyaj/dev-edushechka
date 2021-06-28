CREATE PROCEDURE dbo.[User_SelectById]
	@Id int
AS
	SELECT 
		Name,
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
	WHERE [Id] = @Id