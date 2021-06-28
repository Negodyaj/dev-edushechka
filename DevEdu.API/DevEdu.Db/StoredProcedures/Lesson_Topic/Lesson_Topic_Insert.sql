CREATE PROCEDURE [dbo].[Lesson_Topic_Insert]
@TopicId int,
@LessonId int
AS
INSERT INTO Lesson_Topic (TopicID, ClassId)
VALUES (@TopicId, @LessonId)
SELECT @@IDENTITY

