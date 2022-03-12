CREATE PROCEDURE dbo.User_UpdatePhoto
	@Id int,
	@Photo nvarchar(300)
AS
BEGIN
	UPDATE dbo.[User]
    SET
		Photo = @Photo
    WHERE Id = @Id
END
