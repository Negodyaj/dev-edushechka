CREATE PROCEDURE dbo.Task_Student_Delete
	@TaskId int,
	@StudentId int
AS
BEGIN
	DELETE FROM dbo.Task_Student
	WHERE TaskId = @TaskId AND StudentId = @StudentId
END