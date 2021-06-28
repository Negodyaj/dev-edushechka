CREATE PROCEDURE dbo.Comment_Delete
	@Id int
AS
BEGIN
    UPDATE dbo.Comment
    SET
        IsDeleted = 1
    WHERE Id = @Id
END