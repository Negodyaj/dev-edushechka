CREATE PROCEDURE dbo.Student_Lesson_SelectByLessonAndUserId
    @UserId int,
    @LessonId int
AS
BEGIN
	SELECT 
		Id,
		IsPresent,
		Feedback,
		AbsenceReason
	FROM dbo.Student_Lesson
	WHERE UserId = @UserId AND LessonId = @LessonId
END