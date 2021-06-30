CREATE PROCEDURE [dbo].[Lesson_Comment_Insert]
	@LessonId int,
	@CommentId int
AS
BEGIN
	INSERT [dbo].[Lesson_Comment] (LessonId, CommentId)
	VALUES(@LessonId, @CommentId)
END