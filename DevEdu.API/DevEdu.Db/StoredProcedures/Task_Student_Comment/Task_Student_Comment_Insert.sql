CREATE PROCEDURE [dbo].[Task_Student_Comment_Insert] 
	@TaskStudentId int, 
	@CommentId int
AS 
	INSERT INTO Task_Student_Comment ([TaskStudentId], [CommentId]) 
	VALUES (@TaskStudentId, @CommentId) 
	SELECT @@IDENTITY