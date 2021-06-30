CREATE PROCEDURE dbo.Task_Student_Comment_Insert
	@TaskStudentId int, 
	@CommentId int
AS 
BEGIN
	INSERT INTO dbo.Task_Student_Comment (TaskStudentId, CommentId) 
	VALUES (@TaskStudentId, @CommentId) 
END