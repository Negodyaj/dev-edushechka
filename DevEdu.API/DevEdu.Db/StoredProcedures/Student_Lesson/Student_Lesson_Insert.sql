CREATE PROCEDURE [dbo].[Student_Lesson_Insert]
	@UserId int,
	@LessonId int
AS
	INSERT INTO [Student_Lesson] ([UserId],[LessonId])
	VALUES (@UserId,@LessonId)
	SELECT @@IDENTITY