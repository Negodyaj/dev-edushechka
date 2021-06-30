CREATE PROCEDURE dbo.User_Update
	@Id				int,
	@Name			nvarchar(50) NULL,
	@Username		nvarchar(50) NULL,
	@CityId			int NULL,
	@GitHubAccount	nvarchar(50) NULL,
	@Photo			nvarchar(150) NULL,
	@PhoneNumer		nvarchar(12) NULL
AS
BEGIN
	UPDATE dbo.[User]
    SET
		Name			= @Name,
		Username		= @Username,
		CityId			= @CityId,
		GitHubAccount	= @GitHubAccount,
		Photo			= @Photo,
		PhoneNumer		= @PhoneNumer
    WHERE Id = @Id
END