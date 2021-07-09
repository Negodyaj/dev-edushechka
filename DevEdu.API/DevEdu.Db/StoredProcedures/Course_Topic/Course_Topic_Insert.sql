CREATE PROCEDURE [dbo].[Course_Topic_Insert]
	@CourseId int,
	@TopicId int,
	@Position int
AS
BEGIN
	INSERT dbo.Course_Topic (CourseId, TopicId, Position)
	VALUES (@CourseId, @TopicId, @Position)
	SELECT @@IDENTITY 
END