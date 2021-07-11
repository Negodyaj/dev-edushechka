CREATE PROCEDURE [dbo].[Lesson_SelectById]
	@Id int
AS
BEGIN
	SELECT 
		l.Id, 
		l.Date, 
		l.TeacherComment, 
		l.IsDeleted,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
	FROM dbo.Lesson as l
		inner join dbo.[User] u on l.TeacherId = u.Id  
	WHERE l.Id = @Id
END