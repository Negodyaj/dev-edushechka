CREATE PROCEDURE dbo.Task_Student_UpdateStatusId
	@TaskId int,
	@StudentId int,
	@StatusId int,
	@CompletedDate datetime = null
AS
BEGIN
	UPDATE Task_Student
	SET 
		StatusId = @StatusId,
		CompletedDate = @CompletedDate
	WHERE TaskId = @TaskId AND StudentId = @StudentId
END