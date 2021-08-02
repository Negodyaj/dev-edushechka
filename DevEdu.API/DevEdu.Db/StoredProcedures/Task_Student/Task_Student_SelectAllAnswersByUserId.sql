CREATE PROCEDURE dbo.Task_Student_SelectAllAnswersByUserId
	@UserId int
AS
BEGIN
	SELECT
		tstud.Id,
		tstud.TaskId,
		tstud.Answer,
		tstud.CompletedDate,
		tstud.StatusId as Id
	FROM dbo.Task_Student tstud
		LEFT JOIN dbo.[User] us on us.Id = tstud.StudentId
		LEFT JOIN dbo.Task t on t.Id = tstud.TaskId
	WHERE tstud.StudentId =  @UserId AND us.IsDeleted = 0
END
