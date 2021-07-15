CREATE PROCEDURE [dbo].[Lesson_Comments_SelectById]
	@Id int
AS
BEGIN
	SELECT 
		l.Id, 
		c.Id,
		c.UserId,
		c.Text,
		c.Date
	FROM dbo.Lesson as l
		left join dbo.Lesson_Comment lc on lc.LessonId = l.Id  
		left join dbo.Comment c on c.Id = lc.CommentId

	WHERE l.Id = @Id
END