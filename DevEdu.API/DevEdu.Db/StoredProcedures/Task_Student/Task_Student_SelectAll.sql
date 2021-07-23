CREATE PROCEDURE dbo.Task_Student_SelectAll
AS
BEGIN
	SELECT
		tstud.Id,
		tstud.TaskId,
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
END