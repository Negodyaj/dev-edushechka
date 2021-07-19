CREATE PROCEDURE dbo.Task_SelectAnswersByTaskId
	@Id int
AS
BEGIN
SELECT
		ts.Id,
		tss.Name as Status,
		ts.Answer,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
	From dbo.Task_Student ts
		left join dbo.[User] u on u.Id = ts.StudentId
		left join dbo.TaskStatus tss on tss.Id = ts.StatusId
	WHERE 
	ts.TaskId = @Id
END