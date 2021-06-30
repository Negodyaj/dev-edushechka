CREATE PROCEDURE dbo.Topic_Delete
	@Id int
AS
BEGIN
    UPDATE dbo.Topic
    SET
    IsDeleted = 1
    WHERE Id = @Id
END
