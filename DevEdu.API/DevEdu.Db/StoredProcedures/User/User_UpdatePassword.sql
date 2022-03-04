CREATE PROCEDURE dbo.User_UpdatePassword
	@Id	int,
	@Password nvarchar(150)
AS
BEGIN
	UPDATE dbo.[User]
    SET
		Password = @Password
    WHERE Id = @Id
END