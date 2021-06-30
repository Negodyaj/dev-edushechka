CREATE PROCEDURE [dbo].[Course_Topic_Insert]
	@CourseId int,
	@TopicId int
AS
BEGIN
	INSERT dbo.Course_Topic (CourseId, TopicId)
	VALUES (@CourseId, @TopicId)
END