﻿CREATE PROCEDURE dbo.Task_Student_SelectByTaskAndStudent
	@TaskId int,
	@StudentId int
AS
BEGIN
 	SELECT Id, TaskId, StudentId, StatusId, Answer FROM dbo.Task_Student
	WHERE TaskId = @TaskId AND StudentId = @StudentId
END