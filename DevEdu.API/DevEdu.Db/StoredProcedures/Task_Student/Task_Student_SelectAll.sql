CREATE PROCEDURE dbo.Task_Student_SelectAll
AS
BEGIN
	SELECT
		Id,
		TaskId,
		StudentId,
		Answer,
		CompletedDate,
		StatusId as Id
	FROM dbo.Task_Student
END