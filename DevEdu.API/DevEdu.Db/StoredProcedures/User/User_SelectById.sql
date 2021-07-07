CREATE PROCEDURE dbo.User_SelectById
	@Id int
AS
BEGIN
	SELECT 
		Id,
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
	WHERE Id = @Id
END