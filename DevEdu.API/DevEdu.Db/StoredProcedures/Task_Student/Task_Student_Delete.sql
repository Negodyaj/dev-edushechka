CREATE PROCEDURE dbo.Task_Student_Delete
	@TaskId int,
	@StudentId int
AS
BEGIN
    UPDATE dbo.Task_Student
    SET
        IsDeleted = 1
    WHERE TaskId = @TaskId AND StudentId = @StudentId
END