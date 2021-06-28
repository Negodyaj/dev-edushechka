CREATE PROCEDURE dbo.Course_Delete
	@Id int
AS
BEGIN
    UPDATE dbo.Course
    SET
        IsDeleted = 1
    WHERE Id = @Id
END