CREATE PROCEDURE [dbo].[Student_Lesson_SelectByLessonId]
	@LessonId int
AS
BEGIN
	SELECT 
		sl.Id,
		sl.AttendanceType,
		sl.Feedback,
		sl.AbsenceReason,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
	FROM dbo.Student_Lesson as sl 
	inner join dbo.[User] u on u.Id = sl.UserId

	WHERE sl.LessonId = @LessonId
END