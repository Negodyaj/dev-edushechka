CREATE PROCEDURE dbo.Student_Lesson_SelectByLessonAndUserId
    @UserId int,
    @LessonId int
AS
BEGIN
	SELECT 
		sl.Id,		
		IsPresent,
		Feedback,
		AbsenceReason,
		l.Id,
		u.Id
	FROM dbo.Student_Lesson sl
	inner join dbo.Lesson l on LessonId=l.Id
	inner join dbo.[User] u on UserId=u.Id
	WHERE UserId = @UserId AND LessonId = @LessonId
END