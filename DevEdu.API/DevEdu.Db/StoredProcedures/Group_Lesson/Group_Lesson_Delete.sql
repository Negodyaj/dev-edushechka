CREATE PROCEDURE [dbo].[Group_Lesson_Delete]
	@LessonId int,
	@GroupId int
AS
BEGIN
	DELETE FROM [dbo].[Group_Lesson]
	WHERE GroupId = @GroupId AND LessonId = @LessonId
END