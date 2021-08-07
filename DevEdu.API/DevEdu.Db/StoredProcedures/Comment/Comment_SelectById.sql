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
		ts.Id as Id,
		ts.Answer,
		ts.CompletedDate
	FROM dbo.Comment c
		inner join [User] u on u.Id=c.UserId
		inner join User_Role ur on ur.UserId=u.Id
		left join Lesson l on l.Id=c.LessonId
		left join Task_Student ts on ts .Id=c.TaskStudentId
	WHERE (c.Id = @Id)
END