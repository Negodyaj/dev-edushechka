CREATE PROCEDURE [dbo].[Group_Lesson_Delete]
	@LessonId int,
	@GroupId int
AS
	DELETE FROM [dbo].[Group_Lesson]
	OUTPUT DELETED.Id
	WHERE GroupId = @GroupId AND LessonId = @LessonId
