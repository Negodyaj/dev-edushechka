CREATE PROCEDURE dbo.Student_Lesson_UpdateIsPresent
	@UserId int,
	@LessonId int,
	@AttendanceType int
	
AS
BEGIN
	UPDATE dbo.Student_Lesson
	SET
    AttendanceType = @AttendanceType  
	WHERE UserId = @UserId AND LessonId = @LessonId
END