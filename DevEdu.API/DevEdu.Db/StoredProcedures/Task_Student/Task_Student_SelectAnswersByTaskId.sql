CREATE PROCEDURE dbo.Task_Student_SelectAllAnswersByTaskId
	@TaskId int
AS
BEGIN
	SELECT
		tstud.Id,
		tstud.StudentId,
		tstud.Answer,
		tstud.CompletedDate,
		tstud.IsDeleted,
		tstud.StatusId as Id,
		us.Id,
		us.Username,
		us.FirstName,
		us.LastName,
		us.Email,
		us.GitHubAccount,
		us.Photo
	FROM dbo.Task_Student tstud
		inner JOIN dbo.[User] us on us.Id = tstud.StatusId
	WHERE tstud.TaskId = @TaskId
END
