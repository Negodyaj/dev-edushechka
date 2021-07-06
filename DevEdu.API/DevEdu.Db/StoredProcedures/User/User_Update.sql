CREATE PROCEDURE dbo.User_Update
	@Id				int,
	@FisrtName		nvarchar(50) NULL,
	@LastName		nvarchar(50) NULL,
	@Username		nvarchar(50) NULL,
	@CityId			int NULL,
	@GitHubAccount	nvarchar(50) NULL,
	@Photo			nvarchar(150) NULL,
	@PhoneNumer		nvarchar(12) NULL
AS
BEGIN
	UPDATE dbo.[User]
    SET
		FisrtName		= @FisrtName,
		LastName		= @LastName,
		Username		= @Username,
		CityId			= @CityId,
		GitHubAccount	= @GitHubAccount,
		Photo			= @Photo,
		PhoneNumer		= @PhoneNumer
    WHERE Id = @Id
END