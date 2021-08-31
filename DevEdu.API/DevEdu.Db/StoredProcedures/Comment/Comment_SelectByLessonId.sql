CREATE PROCEDURE [dbo].[Comment_SelectByLessonId]
	@LessonId int
AS
BEGIN
	SELECT 
		c.Id,
		c.Text,
		c.Date,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
	FROM dbo.Comment as c
		inner join Lesson l on l.Id = @LessonId 
		inner join [User] u on u.Id = c.UserId
END