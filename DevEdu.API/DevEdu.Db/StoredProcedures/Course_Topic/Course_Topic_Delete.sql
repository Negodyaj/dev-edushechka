CREATE PROCEDURE [dbo].[Course_Topic_Delete]
	@CourseId int,
	@TopicId int
AS
BEGIN
	DELETE FROM dbo.Course_Topic
	WHERE CourseId = @CourseId AND TopicId = @TopicId
END