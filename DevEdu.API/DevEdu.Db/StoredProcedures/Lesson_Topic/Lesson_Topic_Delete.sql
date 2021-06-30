CREATE PROCEDURE dbo.Lesson_Topic_Delete
	@TopicId int,
	@LessonId int
AS
BEGIN
	DELETE FROM dbo.Lesson_Topic
	WHERE TopicId = @TopicId AND ClassId = @LessonId
END