CREATE PROCEDURE dbo.User_Insert
	@FirstName			nvarchar(50),
	@LastName			nvarchar(50),
	@Patronymic 		nvarchar(50),
	@Email				nvarchar(50),
	@Username			nvarchar(50),
	@Password			nvarchar(30),
	@ContractNumber		nvarchar(50),
	@CityId				int,
	@BirthDate			date,
	@GitHubAccount		nvarchar(50) NULL,
	@Photo				nvarchar(150),
	@PhoneNumber			nvarchar(12)
AS
BEGIN
	INSERT INTO dbo.[User] ([FirstName],[LastName],[Patronymic],[Email],[Username],[Password],[RegistrationDate],[ContractNumber],[CityId],
		[BirthDate],[GitHubAccount],[Photo],[PhoneNumber])
	VALUES (@FirstName,@LastName,@Patronymic,@Email,@Username,@Password,getdate(),@ContractNumber,@CityId,
		@BirthDate,@GitHubAccount,@Photo,@PhoneNumber)
	SELECT @@IDENTITY
END