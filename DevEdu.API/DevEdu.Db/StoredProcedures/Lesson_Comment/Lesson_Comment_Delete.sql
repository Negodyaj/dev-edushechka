CREATE PROCEDURE [dbo].[Lesson_Comment_Delete]
	@LessonId int,
	@CommentId int
AS
	DELETE FROM [dbo].[Lesson_Comment]
	WHERE LessonId = @LessonId AND CommentId = @CommentId

