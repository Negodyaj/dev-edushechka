CREATE PROCEDURE dbo.Task_Student_SelectAll
AS
BEGIN
	SELECT
		Id,
		TaskId,
		StudentId,
		Answer,
		StatusId as Id
	FROM dbo.Task_Student
END