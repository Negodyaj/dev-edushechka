CREATE PROCEDURE dbo.Task_Student_GetIdByTaskAndStudent
	@TaskId int,
	@StudentId int
AS
BEGIN
 	SELECT Id FROM dbo.Task_Student
	WHERE TaskId = @TaskId AND StudentId = @StudentId
END
