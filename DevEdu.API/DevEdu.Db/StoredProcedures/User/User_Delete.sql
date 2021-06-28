CREATE PROCEDURE dbo.User_Delete
	@Id int
AS
    UPDATE dbo.[User]
    SET
        IsDeleted = 1
    WHERE Id = @Id