CREATE PROCEDURE [dbo].[Task_Student_UpdateAnswer]
	@TaskId int,
	@StudentId int,
	@Answer nvarchar(500)
AS
	UPDATE [Task_Student]
	SET 
		[Answer] = @Answer
	WHERE [TaskId] = @TaskId AND [StudentId] = @StudentId