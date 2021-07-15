CREATE PROCEDURE [dbo].[Lesson_SelectById]
	@Id int
AS
BEGIN
	SELECT 
		l.Id, 
		l.Date, 
		l.TeacherComment,
		l.LinkToRecord,
		l.IsDeleted,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo,
		t.Id,
		t.Name
	FROM dbo.Lesson as l
		inner join dbo.[User] u on l.TeacherId = u.Id  

		inner join dbo.Lesson_Topic lt on l.Id = lt.LessonId
		inner join dbo.Topic t on t.Id = lt.Id
	WHERE l.Id = @Id
END