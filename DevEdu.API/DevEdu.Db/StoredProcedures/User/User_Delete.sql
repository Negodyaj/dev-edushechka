CREATE PROCEDURE dbo.User_Delete
	@Id int
AS
BEGIN
    UPDATE dbo.[User]
    SET
        IsDeleted = 1
    WHERE Id = @Id
END