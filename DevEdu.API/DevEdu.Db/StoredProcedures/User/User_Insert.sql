CREATE PROCEDURE dbo.User_Insert
	@FirstName			nvarchar(50),
	@LastName			nvarchar(50),
	@Patronymic 		nvarchar(50),
	@Email				nvarchar(50),
	@Username			nvarchar(50),
	@Password			nvarchar(200),
	@CityId				int,
	@BirthDate			date,
	@GitHubAccount		nvarchar(50)	NULL,
	@PhoneNumber		nvarchar(12)
AS
BEGIN
	INSERT INTO dbo.[User]  ([FirstName],[LastName],[Patronymic],[Email],[Username],[Password],[RegistrationDate],[CityId],
		[BirthDate],[GitHubAccount],[PhoneNumber])
	VALUES (@FirstName,@LastName,@Patronymic,@Email,@Username,@Password,getdate(),@CityId,
		@BirthDate,@GitHubAccount,@PhoneNumber)
	SELECT @@IDENTITY
END