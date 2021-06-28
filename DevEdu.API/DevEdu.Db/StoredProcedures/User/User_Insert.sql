CREATE PROCEDURE [dbo].[User_Insert]
	@Name				nvarchar(50),
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
	INSERT INTO dbo.[User] ([Name],[Email],[Username],[Password],[RegistrationDate],[ContractNumber],[CityId],
		[BirthDate],[GitHubAccount],[Photo],[PhoneNumer])
	VALUES (@Name,@Email,@Username,@Password,getdate(),@ContractNumber,@CityId,
		@BirthDate,@GitHubAccount,@Photo,@PhoneNumer)
	SELECT @@IDENTITY