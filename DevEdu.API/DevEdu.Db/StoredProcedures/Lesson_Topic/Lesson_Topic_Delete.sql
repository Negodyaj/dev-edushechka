CREATE PROCEDURE [dbo].[Lesson_Topic_Delete]
@TopicId int,
@LessonId int
AS
DELETE FROM Lesson_Topic
WHERE TopicId = @TopicId AND ClassId = @LessonId
