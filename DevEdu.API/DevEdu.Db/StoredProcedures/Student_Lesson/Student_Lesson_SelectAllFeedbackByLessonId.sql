CREATE PROCEDURE dbo.Student_Lesson_SelectAllFeedbackByLessonId
@LessonId Int	
AS
BEGIN	
SELECT 
		s.Id,
		s.Feedback,
		s.AttendanceType,
		s.AbsenceReason,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Email,
		u.Photo
FROM dbo.Student_Lesson as s
		inner join [User] u on u.Id=s.UserId
WHERE s.LessonId=@LessonId
END
