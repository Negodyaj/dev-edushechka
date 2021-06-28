CREATE PROCEDURE [dbo].[Lesson_Comment_Insert]
	@LessonId int,
	@CommentId int
AS
	INSERT [dbo].[Lesson_Comment]
	VALUES(@LessonId, @CommentId)
	SELECT @@IDENTITY
