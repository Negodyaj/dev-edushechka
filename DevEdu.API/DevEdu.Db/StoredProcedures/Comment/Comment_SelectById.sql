CREATE PROCEDURE dbo.Comment_SelectById
	@Id int
AS
BEGIN
	SELECT 
		c.Id,
		c.Text,
		c.Date,
		c.IsDeleted,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo,
		ur.RoleId as Id,
		l.Id as Id,
		l.Date,
		l.TeacherComment,
		sh.Id as Id,
		sh.Answer,
		sh.CompletedDate
	FROM dbo.Comment c
		inner join [User] u on u.Id=c.UserId
		inner join User_Role ur on ur.UserId=u.Id
		left join Lesson l on l.Id=c.LessonId
		left join Student_Homework sh on sh.Id=c.StudentHomeworkId
	WHERE (c.Id = @Id)
END