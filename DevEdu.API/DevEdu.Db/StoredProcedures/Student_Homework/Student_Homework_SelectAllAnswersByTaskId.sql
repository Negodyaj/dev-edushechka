CREATE PROCEDURE dbo.Student_Homework_SelectAllAnswersByTaskId
	@TaskId int
AS
BEGIN
	SELECT
		sh.Id,
		sh.Answer,
		sh.CompletedDate,
		sh.Rating,
		sh.StatusId as Id,
		u.Id,
		u.Username,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
	FROM Homework h
		inner JOIN Student_Homework sh on sh.HomeworkId = h.Id
		inner JOIN [User] u on u.Id = sh.StudentId
	WHERE h.TaskId = @TaskId
END