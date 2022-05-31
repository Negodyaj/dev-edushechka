﻿CREATE PROCEDURE dbo.Student_Homework_SelectAnswerByTaskIdAndUserId
	@TaskId int,
	@UserId int
AS
BEGIN
	SELECT
		sh.Id,
		sh.Answer,
		sh.CompletedDate,
		sh.AnswerDate,
		h.Id,
		h.StartDate,
		h.EndDate,
		t.Id,
		t.[Name],
		t.[Description],
		t.Links,
		t.IsRequired,
		t.IsDeleted,
		sh.StatusId as Id
	FROM Student_Homework sh
		inner join Homework h on h.Id = sh.HomeworkId
		inner join Task t on t.Id = h.TaskId
	WHERE h.TaskId = @TaskId and sh.StudentId = @UserId
END