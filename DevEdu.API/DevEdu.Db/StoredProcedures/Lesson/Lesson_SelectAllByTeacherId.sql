CREATE PROCEDURE [dbo].[Lesson_SelectAllByTeacherId]
	@TeacherId int
AS
BEGIN
		SELECT 
		l.Id, 
		l.Date, 
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo,
		t.Id,
		t.Name,
		c.Id,
		c.Name,
		c.Description
	FROM dbo.Lesson as l
		inner join dbo.[User] u on l.TeacherId = u.Id  

		left join dbo.Lesson_Topic lt on l.Id = lt.LessonId
		left join dbo.Topic t on t.Id = lt.TopicId

		inner join dbo.Group_Lesson gl on gl.LessonId = l.Id
		inner join dbo.[Group] g on g.Id = gl.GroupId
		inner join dbo.Course c on g.CourseId = c.Id

	WHERE l.TeacherId = @TeacherId and l.IsDeleted = 0
END