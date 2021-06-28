CREATE PROCEDURE [dbo].[Task_Student_Delete]
	@TaskId int,
	@StudentId int,
	@StatusId int
AS
	DELETE FROM [dbo].[Task_Student]
	WHERE [TaskId] = @TaskId AND [StudentId] = @StudentId AND [StatusId] = @StatusId
