CREATE PROCEDURE dbo.Task_Student_Comment_Delete
	@TaskStudentId int,
	@CommentId int
AS
BEGIN
	DELETE FROM dbo.Task_Student_Comment
	WHERE TaskStudentId = @TaskStudentId AND CommentId = @CommentId
END