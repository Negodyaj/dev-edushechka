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
		u.Patronymic,
		u.Email,
		u.Username,
		u.Password,
		u.RegistrationDate,
		u.ContractNumber,
		u.CityId,
		u.BirthDate,
		u.GitHubAccount,
		u.Photo,
		u.PhoneNumber,
		u.ExileDate,
		c.Id,
		c.Text,
		t.Id, 
		t.Name,
		t.Duration,
		g.Id, 
		g.CourseId, 
		g.GroupStatusId, 
		g.StartDate, 
		g.Timetable, 
		g.PaymentPerMonth,
		us.Id,
		us.FirstName,
		us.LastName,
		us.Patronymic,
		us.Email,
		us.Username,
		us.Password,
		us.RegistrationDate,
		us.ContractNumber,
		us.CityId,
		us.BirthDate,
		us.GitHubAccount,
		us.Photo,
		us.PhoneNumber,
		us.ExileDate
	FROM dbo.Lesson as l
		inner join dbo.[User] u on l.TeacherId = u.Id  
		left outer join dbo.Lesson_Comment ls on l.Id = ls.LessonId
		left outer join dbo.Comment c on ls.CommentId = c.Id
		inner join dbo.Lesson_Topic lt on l.Id = lt.LessonId
		inner join dbo.Topic t on t.Id = lt.TopicId
		inner join dbo.Group_Lesson gl on l.Id = gl.LessonId
		inner join dbo.[Group] g on gl.GroupId = g.Id
		inner join dbo.Student_Lesson sl on l.Id = sl.LessonId
		inner join dbo.[User] us on sl.UserId = us.Id
	WHERE l.IsDeleted = 0
END