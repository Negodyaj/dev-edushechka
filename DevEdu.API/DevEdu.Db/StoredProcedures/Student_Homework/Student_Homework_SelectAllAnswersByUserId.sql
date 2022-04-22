CREATE PROCEDURE dbo.Student_Homework_SelectAllAnswersByUserId
	@UserId int
AS
BEGIN
	SELECT
		sh.Id,
		sh.Answer,
		sh.CompletedDate,
		sh.AnswerDate,
		sh.StatusId as Id,
		h.Id,
		h.StartDate,
		h.EndDate,
		t.Id,
		t.Name,
		t.Description
	FROM Student_Homework sh
		inner join Homework h on h.Id = sh.HomeworkId
		inner join Task t on t.Id = h.TaskId
	WHERE sh.StudentId =  @UserId
END