CREATE PROCEDURE [dbo].[Group_Lesson_Insert]
	@GroupId int,
	@LessonId int
	
AS
BEGIN
	INSERT dbo.Group_Lesson (GroupId, LessonId)
	VALUES (@GroupId, @LessonId)
END