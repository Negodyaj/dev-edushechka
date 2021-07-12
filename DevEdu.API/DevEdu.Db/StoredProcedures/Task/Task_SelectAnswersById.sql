CREATE PROCEDURE dbo.Task_SelectAnswersById
	@Id int
AS
BEGIN
SELECT
		ts.Id,
		u.FirstName as StudentFirstName,
		u.LastName as StudentLastName,
		tss.Name as Status,
		ts.Answer
	From dbo.Task t
		left join dbo.Task_Student ts on ts.TaskId = t.Id
		left join dbo.[User] u on u.Id = ts.StudentId
		left join dbo.TaskStatus tss on tss.Id = ts.StatusId
	WHERE 
	t.Id = @Id
END