CREATE PROCEDURE dbo.Task_Student_UpdateStatusId
	@TaskId int,
	@StudentId int,
	@StatusId int
AS
BEGIN
	UPDATE Task_Student
	SET 
		StatusId = @StatusId
	WHERE TaskId = @TaskId AND StudentId = @StudentId
END