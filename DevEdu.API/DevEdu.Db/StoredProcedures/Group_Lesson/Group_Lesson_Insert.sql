CREATE PROCEDURE [dbo].[Group_Lesson_Insert]
	@LessonId int,
	@GroupId int
AS
BEGIN
	INSERT dbo.Group_Lesson (LessonId, GroupId)
	VALUES (@LessonId, @GroupId)
END