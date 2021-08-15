CREATE PROCEDURE dbo.User_Update
	@Id				int,
	@FirstName		nvarchar(50)	NULL,
	@LastName		nvarchar(50)	NULL,
	@Patronymic		nvarchar(50)	NULL,
	@Username		nvarchar(50)	NULL,
	@CityId			int				NULL,
	@GitHubAccount	nvarchar(50)	NULL,
	@Photo			nvarchar(150)	NULL,
	@PhoneNumber	nvarchar(12)	NULL
AS
BEGIN
	UPDATE dbo.[User]
    SET
		FirstName		= @FirstName,
		LastName		= @LastName,
		Patronymic		= @Patronymic,
		Username		= @Username,
		CityId			= @CityId,
		GitHubAccount	= @GitHubAccount,
		Photo			= @Photo,
		PhoneNumber		= @PhoneNumber
    WHERE Id = @Id
END