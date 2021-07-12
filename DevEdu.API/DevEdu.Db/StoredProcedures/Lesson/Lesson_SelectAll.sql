CREATE PROCEDURE [dbo].[Lesson_SelectAll]
AS
BEGIN
		SELECT 
		l.Id, 
		l.Date, 
		l.TeacherComment, 
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
	FROM dbo.Lesson as l
		inner join dbo.[User] u on l.TeacherId = u.Id  
	WHERE l.IsDeleted = 0
END