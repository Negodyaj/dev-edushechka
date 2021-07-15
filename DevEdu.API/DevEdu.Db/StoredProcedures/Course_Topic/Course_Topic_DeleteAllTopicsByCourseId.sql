CREATE PROCEDURE [dbo].[Course_Topic_DeleteAllTopicsByCourseId]
@CourseId int	
AS
BEGIN
	DELETE FROM dbo.Course_Topic
	WHERE CourseId = @CourseId 
END