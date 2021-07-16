CREATE PROCEDURE [dbo].[Lesson_SelectAllByGroupId]
	@GroupId int
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
		u.Photo,
		t.Id,
		t.Name
	FROM dbo.Lesson as l
		inner join dbo.[User] u on l.TeacherId = u.Id  

		inner join dbo.Lesson_Topic lt on l.Id = lt.LessonId
		inner join dbo.Topic t on t.Id = lt.Id
		
		inner join dbo.Group_Lesson gl on gl.LessonId = l.Id
	WHERE gl.GroupId = @GroupId and l.IsDeleted = 0
END
