CREATE PROCEDURE dbo.Student_Homework_Delete
	@Id int
AS
BEGIN
    UPDATE Student_Homework
    SET
        IsDeleted = 1
    WHERE Id = @Id
END