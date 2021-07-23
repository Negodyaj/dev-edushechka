CREATE PROCEDURE dbo.Group_Task_Delete
	@GroupId int,
	@TaskId int
AS
BEGIN
	DELETE FROM dbo.Group_Task
	WHERE GroupId = @GroupId AND TaskId = @TaskId
END