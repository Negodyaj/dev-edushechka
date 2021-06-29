CREATE PROCEDURE dbo.Lesson_Topic_Insert
	@TopicId int,
	@LessonId int
AS
BEGIN
	INSERT INTO dbo.Lesson_Topic (TopicId, ClassId)
	VALUES (@TopicId, @LessonId)
END
