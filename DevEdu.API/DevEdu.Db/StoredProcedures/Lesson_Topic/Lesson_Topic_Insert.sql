CREATE PROCEDURE [dbo].[Lesson_Topic_Insert]
@TopicId int,
@LessonId int
AS
INSERT INTO Lesson_Topic (TopicId, ClassId)
VALUES (@TopicId, @LessonId)

