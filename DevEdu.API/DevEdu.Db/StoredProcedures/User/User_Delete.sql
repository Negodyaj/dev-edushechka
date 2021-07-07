CREATE PROCEDURE dbo.User_Delete
	@Id int
AS
BEGIN
    UPDATE dbo.[User]
    SET
        IsDeleted = 1,
        ExileDate = getdate()
    WHERE Id = @Id
END