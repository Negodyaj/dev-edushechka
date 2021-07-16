CREATE PROCEDURE [dbo].[Lesson_Comments_SelectById]
	@LessonId int
AS
BEGIN
	SELECT 
		c.Id,
		c.UserId,
		c.Text,
		c.Date
	FROM dbo.Comment as c
		inner join dbo.Lesson_Comment lc on lc.LessonId = @LessonId 
END