CREATE PROCEDURE dbo.Task_Student_SelectAll
AS
BEGIN
	SELECT Id, TaskId, StudentId, StatusId, Answer FROM dbo.Task_Student
END