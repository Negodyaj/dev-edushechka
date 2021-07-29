CREATE PROCEDURE dbo.Task_Student_SelectByTaskId
	@Id int
AS
BEGIN
SELECT
		ts.Id,
		tss.Name as Status,
		ts.Answer,
		ts.CompletedDate,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
	From dbo.Task_Student ts
		left join dbo.[User] u on u.Id = ts.StudentId
		left join dbo.TaskStatus tss on tss.Id = ts.StatusId
	WHERE 
	ts.TaskId = @Id AND ts.IsDeleted = 0
END