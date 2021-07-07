CREATE PROCEDURE dbo.User_Insert
	@FisrtName			nvarchar(50),
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
	@PhoneNumer			nvarchar(12)
AS
BEGIN
	INSERT INTO dbo.[User] ([FisrtName],[LastName],[Patronymic],[Email],[Username],[Password],[RegistrationDate],[ContractNumber],[CityId],
		[BirthDate],[GitHubAccount],[Photo],[PhoneNumer])
	VALUES (@FisrtName,@LastName,@Patronymic,@Email,@Username,@Password,getdate(),@ContractNumber,@CityId,
		@BirthDate,@GitHubAccount,@Photo,@PhoneNumer)
	SELECT @@IDENTITY
END