CREATE PROCEDURE [dbo].[Course_Topic_Update]
	@CourseId int,
	@TopicId int,
	@Position int
AS
BEGIN
	UPDATE dbo.Course_Topic
	SET
	TopicId = @TopicId,
	Position = @Position
	WHERE CourseId = @CourseId and TopicId = @TopicId
END
