CREATE PROCEDURE dbo.Group_Task_Delete
	@GroupId int,
	@TaskId int
AS
BEGIN
	DELETE FROM dbo.Homework
	WHERE GroupId = @GroupId AND TaskId = @TaskId
END