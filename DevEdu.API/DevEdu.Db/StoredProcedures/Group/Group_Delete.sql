CREATE PROCEDURE dbo.Group_Delete
	@Id int
AS
BEGIN
    UPDATE dbo.[Group]
    SET
        IsDeleted = 1
    WHERE Id = @Id
END