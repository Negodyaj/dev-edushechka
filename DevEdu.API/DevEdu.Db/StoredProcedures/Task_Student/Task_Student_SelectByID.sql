CREATE PROCEDURE [dbo].[Task_Student_SelectByID]
	@TaskId int,
	@StudentId int,
	@StatusId int
AS
 	SELECT * FROM [Task_Student]
	WHERE [TaskId] = @TaskId AND [StudentId] = @StudentId AND [StatusId] = @StatusId