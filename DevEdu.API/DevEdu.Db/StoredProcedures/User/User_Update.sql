CREATE PROCEDURE [dbo].[User_Update]
	@Id				int,
	@Name			nvarchar(50) NULL,
	@Email			nvarchar(50) NULL,
	@Username		nvarchar(50) NULL,
	@Password		nvarchar(30) NULL,
	@CityId			int NULL,
	@GitHubAccount	nvarchar(50) NULL,
	@Photo			nvarchar(150) NULL,
	@PhoneNumer		nvarchar(12) NULL,
	@ExileDate		date NULL
AS
	UPDATE dbo.[User]
    SET
		[Name]			= @Name,
		[Email]			= @Email,
		[Username]		= @Username,
		[Password]		= @Password,
		[CityId]		= @CityId,
		[GitHubAccount] = @GitHubAccount,
		[Photo]			= @Photo,
		[PhoneNumer]	= @PhoneNumer
    WHERE [Id] = @Id